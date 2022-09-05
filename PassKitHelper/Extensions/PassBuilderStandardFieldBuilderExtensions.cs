namespace PassKitHelper
{
    using System;

    /// <summary>
    /// Extensions for <see cref="PassBuilder.StandardFieldBuilder"/>.
    /// </summary>
    public static class PassBuilderStandardFieldBuilderExtensions
    {
        /// <summary>
        /// Optional. Attributed value of the field.
        /// This key’s value overrides the text specified by the value key.
        /// </summary>
        public static PassBuilder.StandardFieldBuilder AttributedValue(this PassBuilder.StandardFieldBuilder builder, string value)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Format string for the alert text that is displayed when the pass is updated.
        /// The format string must contain the escape %@, which is replaced with the field’s new value. For example "Gate changed to %@".
        /// </summary>
        /// <remarks>
        /// If you don’t specify a change message, the user isn’t notified when the field changes.
        /// </remarks>
        public static PassBuilder.StandardFieldBuilder ChangeMessage(this PassBuilder.StandardFieldBuilder builder, string value)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Data detectors that are applied to the field’s value.
        /// </summary>
        [Obsolete("Use overloaded one with DataDetectorType param")]
        public static PassBuilder.StandardFieldBuilder DataDetectorTypes(this PassBuilder.StandardFieldBuilder builder, string[] value)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Data detectors that are applied to the field’s value.
        /// </summary>
        public static PassBuilder.StandardFieldBuilder DataDetectorTypes(this PassBuilder.StandardFieldBuilder builder, DataDetectorType values)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), values.ToPassKitString());
            return builder;
        }

        /// <summary>
        /// Optional. Label text for the field.
        /// </summary>
        public static PassBuilder.StandardFieldBuilder Label(this PassBuilder.StandardFieldBuilder builder, string value)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Alignment for the field’s contents.
        /// </summary>
        [Obsolete("Use overloaded one with TextAlignment param")]
        public static PassBuilder.StandardFieldBuilder TextAlignment(this PassBuilder.StandardFieldBuilder builder, string value)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Alignment for the field’s contents.
        /// </summary>
        public static PassBuilder.StandardFieldBuilder TextAlignmentLeft(this PassBuilder.StandardFieldBuilder builder, TextAlignment value)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), value.ToPassKitString());
            return builder;
        }

        /// <summary>
        /// Required. Value of the field.
        /// </summary>
        public static PassBuilder.StandardFieldBuilder Value(this PassBuilder.StandardFieldBuilder builder, object value)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Style of date to display.
        /// </summary>
        public static PassBuilder.StandardFieldBuilder DateStyle(this PassBuilder.StandardFieldBuilder builder, DateStyle value)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), value.ToPassKitString());
            return builder;
        }

        /// <summary>
        /// Optional. Always display the time and date in the given time zone, not in the user’s current time zone.
        /// </summary>
        public static PassBuilder.StandardFieldBuilder IgnoresTimeZone(this PassBuilder.StandardFieldBuilder builder, bool value)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. If true, the label’s value is displayed as a relative date; otherwise, it is displayed as an absolute date.
        /// The default value is false.
        /// </summary>
        public static PassBuilder.StandardFieldBuilder IsRelative(this PassBuilder.StandardFieldBuilder builder, bool value)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Style of time to display.
        /// </summary>
        public static PassBuilder.StandardFieldBuilder TimeStyle(this PassBuilder.StandardFieldBuilder builder, DateStyle value)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), value.ToPassKitString());
            return builder;
        }

        /// <summary>
        /// ISO 4217 currency code for the field’s value.
        /// </summary>
        public static PassBuilder.StandardFieldBuilder CurrencyCode(this PassBuilder.StandardFieldBuilder builder, string value)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Style of number to display.
        /// </summary>
        public static PassBuilder.StandardFieldBuilder NumberStyle(this PassBuilder.StandardFieldBuilder builder, NumberStyle value)
        {
            builder.SetFieldValue(PassBuilder.GetCaller(), value.ToPassKitString());
            return builder;
        }
    }
}
