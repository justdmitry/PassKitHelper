namespace PassKitHelper
{
    public static class PassBuilderStandardBuilderExtensions
    {
        /// <summary>
        /// Required. Pass type identifier, as issued by Apple.The value must correspond with your signing certificate.
        /// </summary>
        public static PassBuilder.StandardBuilder PassTypeIdentifier(this PassBuilder.StandardBuilder builder, string value)
        {
            builder.SetValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Required. Team identifier of the organization that originated and signed the pass, as issued by Apple.
        /// </summary>
        public static PassBuilder.StandardBuilder TeamIdentifier(this PassBuilder.StandardBuilder builder, string value)
        {
            builder.SetValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Required. Display name of the organization that originated and signed the pass.
        /// </summary>
        public static PassBuilder.StandardBuilder OrganizationName(this PassBuilder.StandardBuilder builder, string value)
        {
            builder.SetValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Required. Serial number that uniquely identifies the pass. No two passes with the same pass type identifier may have the same serial number.
        /// </summary>
        public static PassBuilder.StandardBuilder SerialNumber(this PassBuilder.StandardBuilder builder, string value)
        {
            builder.SetValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Required. Brief description of the pass, used by the iOS accessibility technologies.
        /// </summary>
        /// <remarks>
        /// Don’t try to include all of the data on the pass in its description, just include enough detail to distinguish passes of the same type.
        /// </remarks>
        public static PassBuilder.StandardBuilder Description(this PassBuilder.StandardBuilder builder, string value)
        {
            builder.SetValue(PassBuilder.GetCaller(), value);
            return builder;
        }
    }
}
