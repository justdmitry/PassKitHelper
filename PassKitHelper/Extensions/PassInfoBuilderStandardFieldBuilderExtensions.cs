namespace PassKitHelper
{
    using System;

    public static class PassInfoBuilderStandardFieldBuilderExtensions
    {
        /// <summary>
        /// Optional. Attributed value of the field.
        /// This key’s value overrides the text specified by the value key.
        /// </summary>
        public static PassInfoBuilder.StandardFieldBuilder AttributedValue(this PassInfoBuilder.StandardFieldBuilder builder, string value)
        {
            builder.SetFieldValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Format string for the alert text that is displayed when the pass is updated.
        /// The format string must contain the escape %@, which is replaced with the field’s new value. For example, <code>Gate changed to %@.</code>.
        /// </summary>
        /// <remarks>
        /// If you don’t specify a change message, the user isn’t notified when the field changes.
        /// </remarks>
        public static PassInfoBuilder.StandardFieldBuilder ChangeMessage(this PassInfoBuilder.StandardFieldBuilder builder, string value)
        {
            builder.SetFieldValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Data detectors that are applied to the field’s value.
        /// </summary>
        public static PassInfoBuilder.StandardFieldBuilder DataDetectorTypes(this PassInfoBuilder.StandardFieldBuilder builder, string[] value)
        {
            builder.SetFieldValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Label text for the field.
        /// </summary>
        public static PassInfoBuilder.StandardFieldBuilder Label(this PassInfoBuilder.StandardFieldBuilder builder, string value)
        {
            builder.SetFieldValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Alignment for the field’s contents.
        /// </summary>
        public static PassInfoBuilder.StandardFieldBuilder TextAlignment(this PassInfoBuilder.StandardFieldBuilder builder, string value)
        {
            builder.SetFieldValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Required. Value of the field.
        /// </summary>
        public static PassInfoBuilder.StandardFieldBuilder Value(this PassInfoBuilder.StandardFieldBuilder builder, object value)
        {
            builder.SetFieldValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Style of date to display.
        /// </summary>
        public static PassInfoBuilder.StandardFieldBuilder DateStyle(this PassInfoBuilder.StandardFieldBuilder builder, DateStyle value)
        {
            builder.SetFieldValue(PassInfoBuilder.GetCaller(), value.ToPassKitString());
            return builder;
        }

        /// <summary>
        /// Optional. Always display the time and date in the given time zone, not in the user’s current time zone.
        /// </summary>
        public static PassInfoBuilder.StandardFieldBuilder IgnoresTimeZone(this PassInfoBuilder.StandardFieldBuilder builder, bool value)
        {
            builder.SetFieldValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. If true, the label’s value is displayed as a relative date; otherwise, it is displayed as an absolute date.
        /// The default value is false.
        /// </summary>
        public static PassInfoBuilder.StandardFieldBuilder IsRelative(this PassInfoBuilder.StandardFieldBuilder builder, bool value)
        {
            builder.SetFieldValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Style of time to display.
        /// </summary>
        public static PassInfoBuilder.StandardFieldBuilder TimeStyle(this PassInfoBuilder.StandardFieldBuilder builder, DateStyle value)
        {
            builder.SetFieldValue(PassInfoBuilder.GetCaller(), value.ToPassKitString());
            return builder;
        }

        /// <summary>
        /// ISO 4217 currency code for the field’s value.
        /// </summary>
        public static PassInfoBuilder.StandardFieldBuilder CurrencyCode(this PassInfoBuilder.StandardFieldBuilder builder, string value)
        {
            builder.SetFieldValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Style of number to display.
        /// </summary>
        public static PassInfoBuilder.StandardFieldBuilder NumberStyle(this PassInfoBuilder.StandardFieldBuilder builder, NumberStyle value)
        {
            builder.SetFieldValue(PassInfoBuilder.GetCaller(), value.ToPassKitString());
            return builder;
        }
    }
}
