namespace System
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            var chars = value.ToCharArray();
            chars[0] = char.ToLowerInvariant(chars[0]);
            return new string(chars);
        }
    }
}
