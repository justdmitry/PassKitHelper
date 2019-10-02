namespace PassKitHelper
{
    using System;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    public interface IPassKitService
    {
        /// <summary>
        /// Registering a Device to Receive Push Notifications for a Pass.
        /// </summary>
        /// <param name="deviceLibraryIdentifier">A unique identifier that is used to identify and authenticate this device in future requests.</param>
        /// <param name="passTypeIdentifier">The pass’s type, as specified in the pass.</param>
        /// <param name="serialNumber">The pass’s serial number, as specified in the pass.</param>
        /// <param name="authorizationToken">Pass’s authorization token as specified in the pass.</param>
        /// <param name="pushToken">The push token that the server can use to send push notifications to this device.</param>
        /// <returns>
        /// Response:
        /// If the serial number is already registered for this device, returns HTTP status 200.
        /// If registration succeeds, returns HTTP status 201.
        /// If the request is not authorized, returns HTTP status 401.
        /// Otherwise, returns the appropriate standard HTTP status.
        /// </returns>
        Task<int> RegisterDeviceAsync(string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string authorizationToken, string pushToken);

        /// <summary>
        /// Unregistering a Device.
        /// </summary>
        /// <param name="deviceLibraryIdentifier">A unique identifier that is used to identify and authenticate this device in future requests.</param>
        /// <param name="passTypeIdentifier">The pass’s type, as specified in the pass.</param>
        /// <param name="serialNumber">The pass’s serial number, as specified in the pass.</param>
        /// <param name="authorizationToken">Pass’s authorization token as specified in the pass.</param>
        /// <returns>
        /// Response:
        /// If disassociation succeeds, returns HTTP status 200.
        /// If the request is not authorized, returns HTTP status 401.
        /// Otherwise, returns the appropriate standard HTTP status.
        /// </returns>
        Task<int> UnregisterDeviceAsync(string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string authorizationToken);

        /// <summary>
        /// Getting the Serial Numbers for Passes Associated with a Device.
        /// </summary>
        /// <param name="deviceLibraryIdentifier">A unique identifier that is used to identify and authenticate this device in future requests.</param>
        /// <param name="passTypeIdentifier">The pass’s type, as specified in the pass.</param>
        /// <param name="tag">A tag from a previous request (optional).</param>
        /// <returns>
        /// If the passesUpdatedSince parameter is present, returns only the passes that have been updated since the time indicated by tag. Otherwise, returns all passes.
        ///
        /// Response:
        /// If there are matching passes, returns HTTP status 200 (with array of serialNumbers of updated passes and new tag).
        /// If there are no matching passes, returns HTTP status 204.
        /// Otherwise, returns the appropriate standard HTTP status.
        /// </returns>
        Task<(int status, string[]? passes, string? tag)> GetAssociatedPassesAsync(string deviceLibraryIdentifier, string passTypeIdentifier, string? tag);

        /// <summary>
        /// Getting the Latest Version of a Pass.
        /// </summary>
        /// <param name="passTypeIdentifier">The pass’s type, as specified in the pass.</param>
        /// <param name="serialNumber">The unique pass identifier, as specified in the pass.</param>
        /// <param name="authorizationToken">Pass’s authorization token as specified in the pass.</param>
        /// <param name="ifModifiedSince">Return HTTP status code 304 if the pass has not changed.</param>
        /// <returns>
        /// If request is authorized, returns HTTP status 200 with a payload of the pass data.
        /// If the request is not authorized, returns HTTP status 401 (and null as pass data).
        /// If no data has changed since <see cref="ifModifiedSince"/> - return HTTP status code 304 (and null as pass data).
        /// Otherwise, returns the appropriate standard HTTP status (and null as pass data).
        /// </returns>
        Task<(int statusCode, JObject? passData)> GetPassAsync(string passTypeIdentifier, string serialNumber, string authorizationToken, DateTimeOffset? ifModifiedSince);

        /// <summary>
        /// Logging Errors. Log messages contain a description of the error in a human-readable format.
        /// </summary>
        /// <param name="logs">An array of log messages as strings.</param>
        /// <returns>Completed task.</returns>
        Task ProcessLogsAsync(string[] logs);
    }
}
