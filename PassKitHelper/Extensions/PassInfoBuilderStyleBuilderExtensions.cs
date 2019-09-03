namespace PassKitHelper
{
    using System;

    public static class PassInfoBuilderStyleBuilderExtensions
    {
        /// <summary>
        /// Required for boarding passes; otherwise not allowed. Type of transit.
        /// </summary>
        public static PassInfoBuilder.StyleBuilder TransitType(this PassInfoBuilder.StyleBuilder builder, TransitType value)
        {
            builder.SetStyleValue(PassInfoBuilder.GetCaller(), value.ToPassKitString());
            return builder;
        }
    }
}
