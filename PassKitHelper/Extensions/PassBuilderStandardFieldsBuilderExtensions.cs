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

        public static StandardFieldsBuilder Add(
            this StandardFieldsBuilder builder,
            string key,
            string label,
            string value,
            DataDetectorType dataDetectorTypes)
        {
            return builder.Add(key).Label(label).Value(value).DataDetectorTypes(dataDetectorTypes);
        }

        public static StandardFieldsBuilder Add(
            this StandardFieldsBuilder builder,
            string key,
            string label,
            int value,
            NumberStyle numberStyle)
        {
            return builder.Add(key).Label(label).Value(value).NumberStyle(numberStyle);
        }

        public static StandardFieldsBuilder Add(
            this StandardFieldsBuilder builder,
            string key,
            string label,
            decimal value,
            NumberStyle numberStyle)
        {
            return builder.Add(key).Label(label).Value(value).NumberStyle(numberStyle);
        }
    }
}
