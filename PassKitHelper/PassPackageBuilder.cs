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

    public class PassPackageBuilder : IDisposable
    {
        /// <summary>
        /// Content-Type value for *.pkpass files.
        /// </summary>
        public const string PkpassMimeContentType = "application/vnd.apple.pkpass";

        private readonly PassBuilder passBuilder;
        private readonly X509Certificate2 appleCertificate;
        private readonly X509Certificate2 passCertificate;

        private readonly IDictionary<string, object> files;

        private bool disposed = false;

        public PassPackageBuilder(PassBuilder passBuilder, X509Certificate2 appleCertificate, X509Certificate2 passCertificate)
        {
            this.passBuilder = passBuilder ?? throw new ArgumentNullException(nameof(passBuilder));
            this.appleCertificate = appleCertificate ?? throw new ArgumentNullException(nameof(appleCertificate));
            this.passCertificate = passCertificate ?? throw new ArgumentNullException(nameof(passCertificate));

            if (!this.passCertificate.HasPrivateKey)
            {
                throw new ArgumentException("Pass certificate must have private key", nameof(passCertificate));
            }

            files = new Dictionary<string, object>();
        }

        /// <summary>
        /// If <b>true</b> (default) - will dispose itself (and all files added as streams) when you call <see cref="SignAndBuildAsync()"/>.
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

        public async Task<MemoryStream> SignAndBuildAsync()
        {
            CheckDisposed();

            AddFile("pass.json", passBuilder.Build());

            var manifest = CreateManifestFile();
            AddFile("manifest.json", manifest);

            var signature = CreateSignature(manifest, appleCertificate, passCertificate);
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

        protected MemoryStream CreateManifestFile()
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

#if NETSTANDARD2_0
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(hashes, PassBuilder.JsonSettings);
            return new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
#else
            var ms = new MemoryStream();
            System.Text.Json.JsonSerializer.Serialize(ms, hashes, PassBuilder.JsonOptions);
            ms.Position = 0;
            return ms;
#endif
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

        private void CheckDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(PassPackageBuilder));
            }
        }
    }
}
