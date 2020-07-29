namespace System
{
    using System;
    using System.Globalization;

    public static class DateTimeOffsetExtensions
    {
        public static string ToIsoString(this DateTimeOffset value)
        {
            return value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss\\Z", DateTimeFormatInfo.InvariantInfo);
        }
    }
}
