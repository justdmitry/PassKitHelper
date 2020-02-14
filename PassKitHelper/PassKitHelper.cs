namespace PassKitHelper
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class PassKitHelper : IPassKitHelper
    {
        private readonly PassKitOptions options;
        private readonly Func<HttpClient>? httpClientAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassKitHelper"/> class.
        /// </summary>
        /// <param name="options">Configured options.</param>
        /// <remarks>
        /// Simple constructor, suitable for console app, where HttpClients are "cheap" and may be created/disposed without sideeffects.
        /// </remarks>
        public PassKitHelper(PassKitOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.httpClientAccessor = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PassKitHelper"/> class.
        /// </summary>
        /// <param name="options">Configured options.</param>
        /// <param name="httpClientAccessor">HttpClient accessor/factory.</param>
        /// <remarks>
        /// Constructor for use in webapp (used when <see cref="Microsoft.Extensions.DependencyInjection.PassKitHelperServiceCollectionExtensions.AddPassKitHelper(Microsoft.Extensions.DependencyInjection.IServiceCollection, Action{PassKitOptions})"/> called from your Startup.cs file).
        /// </remarks>
        public PassKitHelper(PassKitOptions options, Func<HttpClient> httpClientAccessor)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.httpClientAccessor = httpClientAccessor;
        }

        /// <inheritdoc cref="IPassKitHelper.CreatePass" />
        public PassBuilder CreateNewPass()
        {
            var p = new PassBuilder();
            options.ConfigureNewPass?.Invoke(p);
            return p;
        }

        /// <inheritdoc cref="IPassKitHelper.CreatePackage(PassInfoBuilder)" />
        public PassPackageBuilder CreateNewPassPackage(PassBuilder passBuilder)
        {
            ValidateOptions();

            var p = new PassPackageBuilder(passBuilder, options.AppleCertificate!, options.PassCertificate!);
            options.ConfigureNewPassPackage?.Invoke(p);
            return p;
        }

        /// <inheritdoc cref="IPassKitHelper.SendPushNotificationAsync(string)" />
        public async Task SendPushNotificationAsync(string pushToken)
        {
            ValidateOptions();

            using var client = httpClientAccessor?.Invoke() ?? CreateNewHttpClient();

            using var content = new StringContent("{\"aps\":\"\"}");

            var req = new HttpRequestMessage(HttpMethod.Post, $"https://api.push.apple.com/3/device/{pushToken}")
            {
                Version = new Version(2, 0),
                Content = content,
            };

            using var response = await client.SendAsync(req);

            response.EnsureSuccessStatusCode();
        }

        protected HttpClient CreateNewHttpClient()
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.ClientCertificates.Add(options.AppleCertificate);
            clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;

            return new HttpClient(clientHandler);
        }

        protected void ValidateOptions()
        {
            if (options.AppleCertificate == null)
            {
                throw new InvalidOperationException("AppleCertificate must not be null");
            }

            if (options.PassCertificate == null)
            {
                throw new InvalidOperationException("PassCertificate must not be null");
            }

            if (!options.PassCertificate.HasPrivateKey)
            {
                throw new InvalidOperationException("PassCertificate must contain private key");
            }
        }
    }
}
