namespace PassKitHelper
{
    public static class PassInfoBuilderAssociatedAppBuilderExtensions
    {
        /// <summary>
        /// Optional. A URL to be passed to the associated app when launching it.
        /// </summary>
        /// <remarks>
        /// The app receives this URL in the application:didFinishLaunchingWithOptions: and application:openURL:options: methods of its app delegate.
        /// If this key is present, the <see cref="AssociatedStoreIdentifiers(PassInfoBuilder.AssociatedAppBuilder, int[])"/> key must also be present.
        /// </remarks>
        public static PassInfoBuilder.AssociatedAppBuilder AppLaunchURL(this PassInfoBuilder.AssociatedAppBuilder builder, string value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. A list of iTunes Store item identifiers for the associated apps.
        /// </summary>
        /// <remarks>
        /// Only one item in the list is used—the first item identifier for an app compatible with the current device.
        /// If the app is not installed, the link opens the App Store and shows the app.
        /// If the app is already installed, the link launches the app.
        /// </remarks>
        public static PassInfoBuilder.AssociatedAppBuilder AssociatedStoreIdentifiers(this PassInfoBuilder.AssociatedAppBuilder builder, int[] values)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), values);
            return builder;
        }
    }
}
