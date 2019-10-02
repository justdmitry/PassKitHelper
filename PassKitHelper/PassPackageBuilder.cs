namespace PassKitHelper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Security.Cryptography;
    using System.Security.Cryptography.Pkcs;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class PassPackageBuilder
    {
        public const string MimeContentType = "application/vnd.apple.pkpass";

        private readonly JObject passInfo;

        private readonly IDictionary<string, object> files;

        public PassPackageBuilder(JObject passInfo)
        {
            this.passInfo = passInfo;
            files = new Dictionary<string, object>();
        }

        public void AddFile(string name, byte[] content)
        {
            files[name] = content;
        }

        public void AddFile(string name, Stream content)
        {
            if (!content.CanSeek)
            {
                throw new ArgumentException(nameof(content), "Stream must support seeking (CanSeek == true)");
            }

            files[name] = content;
        }

        public async Task<MemoryStream> SignAndBuildAsync(Stream appleCertificate, Stream passCertificate, string? passCertificatePassword = null)
        {
            var appleBytes = await StreamToBytesAsync(appleCertificate);
            var passBytes = await StreamToBytesAsync(passCertificate);
            return await SignAndBuildAsync(appleBytes, passBytes, passCertificatePassword);
        }

        public async Task<MemoryStream> SignAndBuildAsync(byte[] appleCertificate, byte[] passCertificate, string? passCertificatePassword = null)
        {
            AddFile("pass.json", Serialize(passInfo));

            var manifest = CreateManifestFile();
            var manifestStream = Serialize(manifest);
            AddFile("manifest.json", manifestStream);

            var signature = CreateSignature(manifestStream, appleCertificate, passCertificate, passCertificatePassword);
            AddFile("signature", signature);

            var ms = new MemoryStream();

            using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                foreach (var file in files)
                {
                    var entry = zip.CreateEntry(file.Key);
                    using var fileStream = entry.Open();
                    switch (file.Value)
                    {
                        case byte[] bytes:
                            await fileStream.WriteAsync(bytes, 0, bytes.Length);
                            break;
                        case Stream stream:
                            stream.Position = 0;
                            await stream.CopyToAsync(fileStream);
                            break;
                        default:
                            throw new Exception("Unknown file content type: " + file.Value.GetType().Name);
                    }
                }
            }

            ms.Position = 0;
            return ms;
        }

        protected JObject CreateManifestFile()
        {
            var hashes = new Dictionary<string, string>();
            using var hasher = SHA1.Create();

            foreach (var file in files)
            {
                switch (file.Value)
                {
                    case byte[] bytes:
                        hashes.Add(file.Key, hasher.ComputeHash(bytes).ToHexString());
                        break;
                    case Stream stream:
                        stream.Position = 0;
                        hashes.Add(file.Key, hasher.ComputeHash(stream).ToHexString());
                        break;
                    default:
                        throw new Exception("Unknown file content type: " + file.Value.GetType().Name);
                }
            }

            return JObject.FromObject(hashes);
        }

        protected byte[] CreateSignature(MemoryStream manifest, byte[] appleCertificate, byte[] passCertificate, string? passCertificatePassword = null)
        {
            var pass509cert = string.IsNullOrEmpty(passCertificatePassword)
                ? new X509Certificate2(passCertificate)
                : new X509Certificate2(passCertificate, passCertificatePassword);

            var content = new SignedCms(new ContentInfo(manifest.ToArray()), true);

            var signer = new CmsSigner(SubjectIdentifierType.SubjectKeyIdentifier, pass509cert)
            {
                IncludeOption = X509IncludeOption.None,
            };

            signer.Certificates.Add(new X509Certificate2(appleCertificate));
            signer.Certificates.Add(pass509cert);
            signer.SignedAttributes.Add(new Pkcs9SigningTime());

            content.ComputeSignature(signer);

            return content.Encode();
        }

        private static MemoryStream Serialize(JObject source)
        {
            var ms = new MemoryStream();

            using (var writer = new StreamWriter(ms, Encoding.UTF8, 2048, true))
            {
                using var jsonWriter = new JsonTextWriter(writer);
                source.WriteTo(jsonWriter);
            }

            ms.Position = 0;
            return ms;
        }

        private static async Task<byte[]> StreamToBytesAsync(Stream stream)
        {
            if (stream is MemoryStream ms)
            {
                return ms.ToArray();
            }

            using var ms2 = new MemoryStream();
            await stream.CopyToAsync(ms2);
            ms2.Position = 0;
            return ms2.ToArray();
        }
    }
}
