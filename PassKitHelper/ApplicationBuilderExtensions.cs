namespace Microsoft.AspNetCore.Builder
{
    using System;
    using Microsoft.AspNetCore.Http;
    using PassKitHelper;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UsePassKitMiddleware(this IApplicationBuilder app, PathString pathMatch)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (!pathMatch.HasValue)
            {
                throw new ArgumentNullException(nameof(pathMatch));
            }

            return app.Map(pathMatch, builder => builder.UseMiddleware<PassKitMiddleware>());
        }
    }
}
