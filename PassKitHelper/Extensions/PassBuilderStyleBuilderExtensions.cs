namespace PassKitHelper
{
    using System;

    public static class PassBuilderStyleBuilderExtensions
    {
        /// <summary>
        /// Required for boarding passes; otherwise not allowed. Type of transit.
        /// </summary>
        public static PassBuilder.StyleBuilder TransitType(this PassBuilder.StyleBuilder builder, TransitType value)
        {
            builder.SetStyleValue(PassBuilder.GetCaller(), value.ToPassKitString());
            return builder;
        }
    }
}
