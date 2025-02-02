namespace PassKitHelper
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Helper class for working with Passes (to create new or send push update for existing).
    /// </summary>
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

        /// <inheritdoc cref="IPassKitHelper.CreateNewPass" />
        public PassBuilder CreateNewPass()
        {
            var p = new PassBuilder();
            options.ConfigureNewPass?.Invoke(p);
            return p;
        }

        /// <inheritdoc cref="IPassKitHelper.CreateNewPassPackage(PassBuilder)" />
        public PassPackageBuilder CreateNewPassPackage(PassBuilder pass)
        {
            ValidateOptions();

            var p = new PassPackageBuilder(pass, options.AppleCertificate!, options.PassCertificate!);
            options.ConfigureNewPassPackage?.Invoke(p);
            return p;
        }

        /// <inheritdoc cref="IPassKitHelper.SendPushNotificationAsync(string)" />
        public async Task<bool> SendPushNotificationAsync(string pushToken)
        {
            ValidateOptions(false);

            using var client = httpClientAccessor?.Invoke() ?? CreateNewHttpClient();

            using var content = new StringContent("{\"aps\":\"\"}");

            var req = new HttpRequestMessage(HttpMethod.Post, $"https://api.push.apple.com/3/device/{pushToken}")
            {
                Version = new Version(2, 0),
                Content = content,
            };

            using var response = await client.SendAsync(req);

            // Code 410 means "Unregistered" ("The device token is inactive for the specified topic")
            // https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/handling_notification_responses_from_apns
            if (response.StatusCode == System.Net.HttpStatusCode.Gone)
            {
                return false;
            }

            // Will throw exception for everything except 200-299.
            response.EnsureSuccessStatusCode();

            return true;
        }

        /// <summary>
        /// Creates new HttpClient. Used when <see cref="PassKitHelper(PassKitOptions)"/> (without httpClientAccessor) was used.
        /// </summary>
        /// <returns>New instances of <see cref="HttpClient"/>.</returns>
        protected HttpClient CreateNewHttpClient()
        {
            var clientHandler = new HttpClientHandler();
            if (options.PassCertificate != null)
            {
                clientHandler.ClientCertificates.Add(options.PassCertificate!);
                clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            }

            return new HttpClient(clientHandler);
        }

        /// <summary>
        /// Validates options before use.
        /// </summary>
        protected void ValidateOptions(bool validateAppleCert = true)
        {
            if (validateAppleCert && options.AppleCertificate == null)
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
