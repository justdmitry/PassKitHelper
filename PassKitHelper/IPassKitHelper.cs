namespace PassKitHelper
{
    using System.Threading.Tasks;

    public interface IPassKitHelper
    {
        /// <summary>
        /// Creates new pass builder (pre-populating it with <see cref="PassKitOptions.ConfigureNewPass"/>).
        /// </summary>
        /// <returns>New pass builder.</returns>
        PassBuilder CreateNewPass();

        /// <summary>
        /// Creates new pass package builder (pre-populating it with <see cref="PassKitOptions.ConfigureNewPassPackage"/>).
        /// </summary>
        /// <returns>New pass package builder.</returns>
        PassPackageBuilder CreateNewPassPackage(PassBuilder pass);

        /// <summary>
        /// Sends notification to Apple server to push-update pass on client device.
        /// </summary>
        /// <param name="pushToken">Token from <see cref="IPassKitService.RegisterDeviceAsync(string, string, string, string, string)"/>.</param>
        /// <returns>Awaitable task, with result: <b>true</b> when push notification was sent succesfully (Apple Server responded with 200) and <b>false</b> when pushToken is no more valid (Apple Server response 410, "The device token is no longer active for the topic").</returns>
        Task<bool> SendPushNotificationAsync(string pushToken);
    }
}
