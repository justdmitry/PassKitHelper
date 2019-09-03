namespace PassKitHelper
{
    public static class PassInfoBuilderCompanionAppBuilderExtensions
    {
        /// <summary>
        /// Optional. Custom information for companion apps. This data is not displayed to the user.
        /// </summary>
        /// <remarks>
        /// For example, a pass for a cafe could include information about the user’s favorite drink and sandwich in a machine-readable form for the companion app to read, making it easy to place an order for “the usual” from the app.
        /// Available in iOS 7.0.
        /// </remarks>
        public static PassInfoBuilder.CompanionAppBuilder UserInfo(this PassInfoBuilder.CompanionAppBuilder builder, object value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }
    }
}
