namespace PassKitHelper
{
    public static class PassInfoBuilderStandardBuilderExtensions
    {
        /// <summary>
        /// Required. Pass type identifier, as issued by Apple.The value must correspond with your signing certificate.
        /// </summary>
        public static PassInfoBuilder.StandardBuilder PassTypeIdentifier(this PassInfoBuilder.StandardBuilder builder, string value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Required. Team identifier of the organization that originated and signed the pass, as issued by Apple.
        /// </summary>
        public static PassInfoBuilder.StandardBuilder TeamIdentifier(this PassInfoBuilder.StandardBuilder builder, string value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Required. Display name of the organization that originated and signed the pass.
        /// </summary>
        public static PassInfoBuilder.StandardBuilder OrganizationName(this PassInfoBuilder.StandardBuilder builder, string value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Required. Serial number that uniquely identifies the pass. No two passes with the same pass type identifier may have the same serial number.
        /// </summary>
        public static PassInfoBuilder.StandardBuilder SerialNumber(this PassInfoBuilder.StandardBuilder builder, string value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Required. Brief description of the pass, used by the iOS accessibility technologies.
        /// </summary>
        /// <remarks>
        /// Don’t try to include all of the data on the pass in its description, just include enough detail to distinguish passes of the same type.
        /// </remarks>
        public static PassInfoBuilder.StandardBuilder Description(this PassInfoBuilder.StandardBuilder builder, string value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }
    }
}
