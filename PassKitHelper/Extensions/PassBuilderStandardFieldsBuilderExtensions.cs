namespace PassKitHelper
{
    using System;
    using static global::PassKitHelper.PassBuilder;

    public static class PassBuilderStandardFieldsBuilderExtensions
    {
        public static StandardFieldsBuilder Add(
            this StandardFieldsBuilder builder,
            string key,
            string label,
            string value)
        {
            return builder.Add(key).Label(label).Value(value);
        }

        public static StandardFieldsBuilder Add(
            this StandardFieldsBuilder builder,
            string key,
            string label,
            DateTimeOffset value,
            DateStyle dateStyle,
            DateStyle timeStyle)
        {
            return builder.Add(key).Label(label).Value(value.ToIsoString()).DateStyle(dateStyle).TimeStyle(timeStyle);
        }
    }
}
