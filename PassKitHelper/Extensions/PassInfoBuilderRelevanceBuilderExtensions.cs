namespace PassKitHelper
{
    using System;

    public static class PassInfoBuilderRelevanceBuilderExtensions
    {
        /// <summary>
        /// Optional. Beacons marking locations where the pass is relevant.
        /// </summary>
        /// <remarks>
        /// Available in iOS 7.0.
        /// </remarks>
        public static PassInfoBuilder.RelevanceBuilder Beacons(this PassInfoBuilder.RelevanceBuilder builder, PassInfoBuilder.Beacon value)
        {
            builder.AppendValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Beacons marking locations where the pass is relevant.
        /// </summary>
        /// <remarks>
        /// Available in iOS 7.0.
        /// </remarks>
        public static PassInfoBuilder.RelevanceBuilder Beacons(this PassInfoBuilder.RelevanceBuilder builder, string proximityUUID, uint? major = null, uint? minor = null, string? relevantText = null)
        {
            var value = new PassInfoBuilder.Beacon(proximityUUID)
            {
                Major = major,
                Minor = minor,
                RelevantText = relevantText,
            };
            return Beacons(builder, value);
        }

        /// <summary>
        /// Optional. Locations where the pass is relevant. For example, the location of your store.
        /// </summary>
        public static PassInfoBuilder.RelevanceBuilder Locations(this PassInfoBuilder.RelevanceBuilder builder, PassInfoBuilder.Location value)
        {
            builder.AppendValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Optional. Locations where the pass is relevant. For example, the location of your store.
        /// </summary>
        public static PassInfoBuilder.RelevanceBuilder Locations(this PassInfoBuilder.RelevanceBuilder builder, double latitude, double longitude, double? altitude = null, string? relevantText = null)
        {
            var value = new PassInfoBuilder.Location
            {
                Latitude = latitude,
                Longitude = longitude,
                Altitude = altitude,
                RelevantText = relevantText,
            };
            return Locations(builder, value);
        }

        /// <summary>
        /// Optional. Maximum distance in meters from a relevant latitude and longitude that the pass is relevant.
        /// This number is compared to the pass’s default distance and the smaller value is used.
        /// </summary>
        /// <remarks>
        /// Available in iOS 7.0.
        /// </remarks>
        public static PassInfoBuilder.RelevanceBuilder MaxDistance(this PassInfoBuilder.RelevanceBuilder builder, uint value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }

        /// <summary>
        /// Recommended for event tickets and boarding passes; otherwise optional.
        /// Date and time when the pass becomes relevant. For example, the start time of a movie.
        /// </summary>
        /// <remarks>
        /// The value must be a complete date with hours and minutes, and may optionally include seconds.
        /// Available in iOS 7.0.
        /// </remarks>
        public static PassInfoBuilder.RelevanceBuilder RelevantDate(this PassInfoBuilder.RelevanceBuilder builder, DateTimeOffset value)
        {
            builder.SetValue(PassInfoBuilder.GetCaller(), value);
            return builder;
        }
    }
}
