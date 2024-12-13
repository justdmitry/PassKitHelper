namespace System
{
    using System.Globalization;

    internal static class DateTimeOffsetExtensions
    {
        public static string ToIsoString(this DateTimeOffset value)
        {
            return value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\\Z", DateTimeFormatInfo.InvariantInfo);
        }
    }
}
