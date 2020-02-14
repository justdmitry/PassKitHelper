namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.Options;
    using PassKitHelper;

    public static class PassKitHelperServiceCollectionExtensions
    {
        private const string HttpFactoryClientName = "PassKitHelper.PushNotificationHttpClient";

        public static IServiceCollection AddPassKitHelper(this IServiceCollection services, Action<PassKitOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services
                .AddHttpClient(HttpFactoryClientName)
                .ConfigurePrimaryHttpMessageHandler(sp =>
                {
                    var opt = sp.GetRequiredService<IOptions<PassKitOptions>>();
                    var handler = new HttpClientHandler();
                    handler.ClientCertificates.Add(opt.Value.AppleCertificate);
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    return handler;
                });

            services.PostConfigure(configureOptions);

            services.AddScoped<IPassKitHelper, PassKitHelper>(sp =>
            {
                HttpClient GetHttpClient() => sp.GetRequiredService<IHttpClientFactory>().CreateClient(HttpFactoryClientName);
                var options = sp.GetRequiredService<IOptions<PassKitOptions>>();
                return new PassKitHelper(options.Value, GetHttpClient);
            });

            return services;
        }
    }
}
