namespace PassKitHelper
{
    public static class PassInfoBuilderWebServiceBuilderExtensions
    {
        /// <summary>
        /// The authentication token to use with the web service. The token must be 16 characters or longer.
        /// </summary>
        public static PassInfoBuilder.WebServiceBuilder AuthenticationToken(this PassInfoBuilder.WebServiceBuilder builder, string value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// The URL of a web service that conforms to the API described in PassKit Web Service Reference.
        /// </summary>
        /// <remarks>
        /// The web service must use the HTTPS protocol; the leading https:// is included in the value of this key.
        /// On devices configured for development, there is UI in Settings to allow HTTP web services.
        /// </remarks>
        public static PassInfoBuilder.WebServiceBuilder WebServiceURL(this PassInfoBuilder.WebServiceBuilder builder, string value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }
    }
}
