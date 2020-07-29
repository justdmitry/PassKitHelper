namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.Options;
    using PassKitHelper;

    /// <summary>
    /// PassKitHelper extensions for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class PassKitHelperServiceCollectionExtensions
    {
        private const string HttpFactoryClientName = "PassKitHelper.PushNotificationHttpClient";

        /// <summary>
        /// Registers PassKitHelper required services (make sure to configure <see cref="PassKitOptions"/> yourself).
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <returns>Same services collection.</returns>
        public static IServiceCollection AddPassKitHelper(this IServiceCollection services)
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
                    handler.ClientCertificates.Add(opt.Value.PassCertificate);
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    return handler;
                });

            services.AddScoped<IPassKitHelper, PassKitHelper>(sp =>
            {
                HttpClient GetHttpClient() => sp.GetRequiredService<IHttpClientFactory>().CreateClient(HttpFactoryClientName);
                var options = sp.GetRequiredService<IOptions<PassKitOptions>>();
                return new PassKitHelper(options.Value, GetHttpClient);
            });

            return services;
        }

        /// <summary>
        /// Registers PassKitHelper required services with <see cref="Action{PassKitOptions}"/> to configure <see cref="PassKitOptions"/>.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <param name="configureOptions">Action to configure <see cref="PassKitOptions"/>.</param>
        /// <returns>Same services collection.</returns>
        public static IServiceCollection AddPassKitHelper(this IServiceCollection services, Action<PassKitOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.PostConfigure(configureOptions);

            return AddPassKitHelper(services);
        }

        /// <summary>
        /// Registers PassKitHelper required services with ready-to-use <see cref="PassKitOptions"/>.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <param name="options">Options to use.</param>
        /// <returns>Same services collection.</returns>
        public static IServiceCollection AddPassKitHelper(this IServiceCollection services, PassKitOptions options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            services.AddSingleton(Options.Create(options));

            return AddPassKitHelper(services);
        }
    }
}
