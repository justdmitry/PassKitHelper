namespace PassKitHelper
{
    using System;

    public static class PassInfoBuilderVisualAppearanceBuilderExtensions
    {
        /// <summary>
        /// Optional. Information specific to the pass’s barcode. The system uses the first valid barcode dictionary in the array. Additional dictionaries can be added as fallbacks.
        /// </summary>
        /// <remarks>
        /// Available in iOS 9.0.
        /// </remarks>
        public static PassInfoBuilder.VisualAppearanceBuilder Barcodes(this PassInfoBuilder.VisualAppearanceBuilder builder, PassInfoBuilder.Barcode value)
        {
            builder.AppendValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Information specific to the pass’s barcode. The system uses the first valid barcode dictionary in the array. Additional dictionaries can be added as fallbacks.
        /// </summary>
        /// <remarks>
        /// Available in iOS 9.0.
        /// </remarks>
        public static PassInfoBuilder.VisualAppearanceBuilder Barcodes(this PassInfoBuilder.VisualAppearanceBuilder builder, string message, BarcodeFormat format, string? altText = null, string? messageEncoding = null)
        {
            var value = new PassInfoBuilder.Barcode(message, format)
            {
                AltText = altText,
            };

            if (messageEncoding != null)
            {
                value.MessageEncoding = messageEncoding;
            }

            return Barcodes(builder, value);
        }

        /// <summary>
        /// Optional. Background color of the pass, specified as an CSS-style RGB triple.
        /// </summary>
        /// <example>Example: <code>rgb(23, 187, 82)</code>.</example>
        public static PassInfoBuilder.VisualAppearanceBuilder BackgroundColor(this PassInfoBuilder.VisualAppearanceBuilder builder, string value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Foreground color of the pass, specified as an CSS-style RGB triple.
        /// </summary>
        /// <example>Example: <code>rgb(23, 187, 82)</code>.</example>
        public static PassInfoBuilder.VisualAppearanceBuilder ForegroundColor(this PassInfoBuilder.VisualAppearanceBuilder builder, string value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional for event tickets and boarding passes; otherwise not allowed.
        /// Identifier used to group related passes. If a grouping identifier is specified, passes with the same style, pass type identifier, and grouping identifier are displayed as a group. Otherwise, passes are grouped automatically.
        /// Use this to group passes that are tightly related, such as the boarding passes for different connections of the same trip.
        /// </summary>
        public static PassInfoBuilder.VisualAppearanceBuilder GroupingIdentifier(this PassInfoBuilder.VisualAppearanceBuilder builder, string value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Color of the label text, specified as a CSS-style RGB triple.
        /// </summary>
        /// <example>Example: <code>rgb(23, 187, 82)</code>.</example>
        public static PassInfoBuilder.VisualAppearanceBuilder LabelColor(this PassInfoBuilder.VisualAppearanceBuilder builder, string value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Text displayed next to the logo on the pass.
        /// </summary>
        public static PassInfoBuilder.VisualAppearanceBuilder LogoText(this PassInfoBuilder.VisualAppearanceBuilder builder, string value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }
    }
}
