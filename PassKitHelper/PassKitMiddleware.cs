namespace PassKitHelper
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Middleware for incoming communications from Apple-servers about Passes, Devices and Registrations.
    /// </summary>
    /// <remarks>
    /// Docs: https://developer.apple.com/library/archive/documentation/PassKit/Reference/PassKit_WebService/WebService.html
    /// </remarks>
    public class PassKitMiddleware
    {
        private readonly RequestDelegate next;

        private readonly ILogger logger;

        public PassKitMiddleware(RequestDelegate next, ILogger<PassKitMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public Task InvokeAsync(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.Request.Path.StartsWithSegments("/v1", out var remainingPath))
            {
                return next(context);
            }

            if (remainingPath.StartsWithSegments("/devices", out var devicesRemainingPath))
            {
                return InvokeDevicesAsync(context, devicesRemainingPath);
            }

            if (remainingPath.StartsWithSegments("/passes", out var passesRemainingPath))
            {
                return InvokePassesAsync(context, passesRemainingPath);
            }

            if (remainingPath == "/log")
            {
                return InvokeLogsAsync(context);
            }

            logger.LogWarning("Unknown path, returning 404: " + remainingPath);
            context.Response.StatusCode = 404;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Process:
        /// Registration:         POST to webServiceURL/version/devices/deviceLibraryIdentifier/registrations/passTypeIdentifier/serialNumber
        /// Get passes for device: GET to webServiceURL/version/devices/deviceLibraryIdentifier/registrations/passTypeIdentifier?passesUpdatedSince=tag
        /// De-registration:    DELETE to webServiceURL/version/devices/deviceLibraryIdentifier/registrations/passTypeIdentifier/serialNumber.
        /// </summary>
        public async Task InvokeDevicesAsync(HttpContext context, PathString devicesRemainingPath)
        {
            var pathParts = devicesRemainingPath.Value.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (pathParts.Length != 3 && pathParts.Length != 4)
            {
                logger.LogWarning($"/devices: wrong number of segments (expected 3 or 4) in {devicesRemainingPath}, returning 400");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            if (!string.Equals(pathParts[1], "registrations", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogWarning($"/devices: unexpected path segment {pathParts[1]} in {devicesRemainingPath}, returning 400");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            var deviceLibraryIdentifier = pathParts[0];
            var passTypeIdentifier = pathParts[2];

            var serialNumber = pathParts.Length == 3 ? null : pathParts[3];
            var authorizationToken = GetAuthorizationToken(context.Request.Headers);

            var service = context.RequestServices.GetRequiredService<IPassKitService>();

            switch (context.Request.Method)
            {
                case "POST": // Registering a Device to Receive Push Notifications for a Pass
                    if (authorizationToken == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }

                    if (serialNumber == null)
                    {
                        logger.LogWarning($"/devices: serialNumber not found in registration: {devicesRemainingPath}, returning 400");
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return;
                    }

                    using (var reader = new StreamReader(context.Request.Body))
                    {
                        var body = await reader.ReadToEndAsync();
                        var payload = JsonConvert.DeserializeObject<RegistrationPayload>(body);

                        if (string.IsNullOrEmpty(payload?.PushToken))
                        {
                            logger.LogWarning($"/devices: pushToken not found, returning 400");
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            return;
                        }

                        var regStatus = await service.RegisterDeviceAsync(deviceLibraryIdentifier, passTypeIdentifier, serialNumber, authorizationToken, payload.PushToken);
                        context.Response.StatusCode = regStatus;
                    }

                    break;

                case "DELETE": // Unregistering a Device
                    if (authorizationToken == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }

                    if (serialNumber == null)
                    {
                        logger.LogWarning($"/devices: serialNumber not found in unregistration: {devicesRemainingPath}, returning 400");
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return;
                    }

                    var unregStatus = await service.UnregisterDeviceAsync(deviceLibraryIdentifier, passTypeIdentifier, serialNumber, authorizationToken);
                    context.Response.StatusCode = unregStatus;

                    break;

                case "GET": // Getting the Serial Numbers for Passes Associated with a Device
                    if (serialNumber != null)
                    {
                        logger.LogWarning($"/devices: extra segment found while getting updated passes: {devicesRemainingPath}, returning 400");
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return;
                    }

                    string? tag = default;
                    if (context.Request.Query.TryGetValue("passesUpdatedSince", out var vs))
                    {
                        tag = vs.ToString();
                    }

                    var (status, passes, newTag) = await service.GetAssociatedPassesAsync(deviceLibraryIdentifier, passTypeIdentifier, tag);

                    if (status == StatusCodes.Status200OK)
                    {
                        var data = new
                        {
                            lastUpdated = newTag,
                            serialNumbers = passes,
                        };

                        context.Response.ContentType = PassInfoBuilder.MimeContentType;
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(data, Formatting.None));
                    }
                    else
                    {
                        context.Response.StatusCode = status;
                    }

                    break;

                default:
                    logger.LogWarning($"/devices: Unknown method: {context.Request.Method}, returning 405");
                    context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                    break;
            }
        }

        /// <summary>
        /// Process GET request to webServiceURL/version/passes/passTypeIdentifier/serialNumber.
        /// </summary>
        public async Task InvokePassesAsync(HttpContext context, PathString passesRemainingPath)
        {
            if (context.Request.Method != "GET")
            {
                logger.LogWarning($"/passes should be GET, but received {context.Request.Method}, returning 405");
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                return;
            }

            var pathParts = passesRemainingPath.Value.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (pathParts.Length != 2)
            {
                logger.LogWarning($"/passes: wrong number of segments (expected 2) in {passesRemainingPath}, returning 400");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            var passTypeIdentifier = pathParts[0];
            var serialNumber = pathParts[1];

            var authorizationToken = GetAuthorizationToken(context.Request.Headers);
            if (authorizationToken == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            DateTimeOffset? ifModifiedSince = null;
            if (context.Request.Headers.TryGetValue("If-Modified-Since", out var ifModifiedSinceValue))
            {
                if (DateTimeOffset.TryParseExact(ifModifiedSinceValue, "R", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedValue))
                {
                    ifModifiedSince = parsedValue;
                }
            }

            var service = context.RequestServices.GetRequiredService<IPassKitService>();
            var (status, pass) = await service.GetPassAsync(passTypeIdentifier, serialNumber, authorizationToken, ifModifiedSince);

            if (status == StatusCodes.Status200OK)
            {
                if (pass == null)
                {
                    throw new Exception("GetPassAsync() must return non-null 'pass' when 'status' == 200");
                }

                context.Response.ContentType = PassInfoBuilder.MimeContentType;
                await context.Response.WriteAsync(pass.ToString(Formatting.None));
            }
            else
            {
                context.Response.StatusCode = status;
            }
        }

        /// <summary>
        /// Process POST request to webServiceURL/version/log.
        /// </summary>
        public async Task InvokeLogsAsync(HttpContext context)
        {
            if (context.Request.Method != "POST")
            {
                logger.LogWarning($"/log should be POST, but received {context.Request.Method}, returning 405");
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                return;
            }

            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var payload = JsonConvert.DeserializeObject<LogsPayload>(body);

            if (payload?.Logs?.Length > 0)
            {
                var service = context.RequestServices.GetRequiredService<IPassKitService>();
                await service.ProcessLogsAsync(payload.Logs);
            }
        }

        protected string? GetAuthorizationToken(IHeaderDictionary headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            if (!headers.TryGetValue("Authorization", out var auth))
            {
                logger.LogWarning("'Authorization' header not found");
                return null;
            }

            const string authPrefix = "ApplePass ";
            var authString = auth.ToString();
            if (!authString.StartsWith(authPrefix, StringComparison.Ordinal))
            {
                logger.LogWarning($"'Authorization' header is invalid: should start with '{authPrefix}'");
                return null;
            }

            var token = authString.Substring(authPrefix.Length);

            if (string.IsNullOrWhiteSpace(token))
            {
                logger.LogWarning($"'Authorization' header is invalid: token is empty");
                return null;
            }

            return token;
        }

        private class LogsPayload
        {
            [JsonProperty("logs")]
            public string[]? Logs { get; set; }
        }

        private class RegistrationPayload
        {
            [JsonProperty("pushToken")]
            public string? PushToken { get; set; }
        }
    }
}
