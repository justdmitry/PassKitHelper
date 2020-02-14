namespace PassKitHelper
{
    public static class PassBuilderWebServiceBuilderExtensions
    {
        /// <summary>
        /// The authentication token to use with the web service. The token must be 16 characters or longer.
        /// </summary>
        public static PassBuilder.WebServiceBuilder AuthenticationToken(this PassBuilder.WebServiceBuilder builder, string value)
        {
            builder.SetValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// The URL of a web service that conforms to the API described in PassKit Web Service Reference.
        /// </summary>
        /// <remarks>
        /// The web service must use the HTTPS protocol; the leading https:// is included in the value of this key.
        /// On devices configured for development, there is UI in Settings to allow HTTP web services.
        /// </remarks>
        public static PassBuilder.WebServiceBuilder WebServiceURL(this PassBuilder.WebServiceBuilder builder, string value)
        {
            builder.SetValue(PassBuilder.GetCaller(), value);
            return builder;
        }
    }
}
