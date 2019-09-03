namespace PassKitHelper
{
    using System;

    public static class PassInfoBuilderExpirationBuilderExtensions
    {
        /// <summary>
        /// Optional. Date and time when the pass expires.
        /// </summary>
        /// <remarks>
        /// The value must be a complete date with hours and minutes, and may optionally include seconds.
        /// Available in iOS 7.0.
        /// </remarks>
        public static PassInfoBuilder.ExpirationBuilder ExpirationDate(this PassInfoBuilder.ExpirationBuilder builder, DateTimeOffset value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Indicates that the pass is void—for example, a one time use coupon that has been redeemed.
        /// </summary>
        /// <remarks>
        /// The default value is false.
        /// Available in iOS 7.0.
        /// </remarks>
        public static PassInfoBuilder.ExpirationBuilder Voided(this PassInfoBuilder.ExpirationBuilder builder, bool value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }
    }
}
