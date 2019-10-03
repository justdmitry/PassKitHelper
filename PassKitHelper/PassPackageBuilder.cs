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

    public class PassPackageBuilder : IDisposable
    {
        /// <summary>
        /// Content-Type value for *.pkpass files.
        /// </summary>
        public const string PkpassMimeContentType = "application/vnd.apple.pkpass";

        private readonly JObject passInfo;

        private readonly IDictionary<string, object> files;

        private bool disposed = false;

        public PassPackageBuilder(JObject passInfo)
        {
            this.passInfo = passInfo;
            files = new Dictionary<string, object>();
        }

        /// <summary>
        /// If <b>true</b> (default) - will dispose itself (and all files added as streams) when you call <see cref="SignAndBuildAsync(byte[], byte[], string)"/> or <see cref="SignAndBuildAsync(Stream, Stream, string)"/>.
        /// </summary>
        public bool AutoDisposeOnBuild { get; set; } = true;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void AddFile(string name, byte[] content)
        {
            CheckDisposed();
            files[name] = content;
        }

        public void AddFile(string name, Stream content)
        {
            CheckDisposed();

            if (!content.CanSeek)
            {
                throw new ArgumentException(nameof(content), "Stream must support seeking (CanSeek == true)");
            }

            files[name] = content;
        }

        public async Task<MemoryStream> SignAndBuildAsync(Stream appleCertificate, Stream passCertificate, string? passCertificatePassword = null)
        {
            CheckDisposed();

            var appleBytes = await StreamToBytesAsync(appleCertificate);
            var passBytes = await StreamToBytesAsync(passCertificate);
            return await SignAndBuildAsync(appleBytes, passBytes, passCertificatePassword);
        }

        public Task<MemoryStream> SignAndBuildAsync(byte[] appleCertificate, byte[] passCertificate, string? passCertificatePassword = null)
        {
            CheckDisposed();

            using var apple509cert = new X509Certificate2(appleCertificate);

            using var pass509cert = string.IsNullOrEmpty(passCertificatePassword)
                ? new X509Certificate2(passCertificate)
                : new X509Certificate2(passCertificate, passCertificatePassword);

            return SignAndBuildAsync(apple509cert, pass509cert);
        }

        public async Task<MemoryStream> SignAndBuildAsync(X509Certificate2 appleCertificate, X509Certificate2 passCertificate)
        {
            CheckDisposed();

            AddFile("pass.json", Serialize(passInfo));

            var manifest = CreateManifestFile();
            var manifestStream = Serialize(manifest);
            AddFile("manifest.json", manifestStream);

            var signature = CreateSignature(manifestStream, appleCertificate, passCertificate);
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

            if (AutoDisposeOnBuild)
            {
                Dispose();
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

        protected byte[] CreateSignature(MemoryStream manifest, X509Certificate2 appleCertificate, X509Certificate2 passCertificate)
        {
            var content = new SignedCms(new ContentInfo(manifest.ToArray()), true);

            var signer = new CmsSigner(SubjectIdentifierType.SubjectKeyIdentifier, passCertificate)
            {
                IncludeOption = X509IncludeOption.None,
            };

            signer.Certificates.Add(appleCertificate);
            signer.Certificates.Add(passCertificate);
            signer.SignedAttributes.Add(new Pkcs9SigningTime());

            content.ComputeSignature(signer);

            return content.Encode();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file.Value is Stream stream)
                        {
                            stream.Dispose();
                        }
                    }
                }
            }

            disposed = true;
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

        private void CheckDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(PassPackageBuilder));
            }
        }
    }
}
