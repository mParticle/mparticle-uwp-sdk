using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class Location
    {
        /// <summary>
        /// Latitude. Required.
        /// </summary>
        [JsonProperty("lat", DefaultValueHandling = DefaultValueHandling.Include)]
        public double Lat;

        /// <summary>
        /// Longitude. Required.
        /// </summary>
        [JsonProperty("lng", DefaultValueHandling = DefaultValueHandling.Include)]
        public double Lng;

        /// <summary>
        /// Horizontal accuracy in meters. Optional.
        /// </summary>
        [JsonProperty("acc", DefaultValueHandling = DefaultValueHandling.Include)]
        public double Accuracy;

        /// <summary>
        /// Vertical accuracy in meters. Optional.  IOS Only.
        /// </summary>
        [JsonProperty("vacc", DefaultValueHandling = DefaultValueHandling.Include)]
        public double VerticalAccuracy;

        /// <summary>
        /// Requested accuracy in meters.  Optional.
        /// </summary>
        [JsonProperty("racc", DefaultValueHandling = DefaultValueHandling.Include)]
        public double RequestedAccuracy;

        /// <summary>
        /// Update interval in meters.  Optional.
        /// </summary>
        [JsonProperty("mdst", DefaultValueHandling = DefaultValueHandling.Include)]
        public double MinimumDistance;

        /// <summary>
        /// Returned by location 'getProvider'. Optional.  Android only.
        /// </summary>
        /// <see cref="http://developer.android.com/reference/android/location/Location.html#getProvider()" />
        /// <remarks>
        /// Will be one of: gps, network, passive, unknown
        /// </remarks>
        [JsonProperty("prv", DefaultValueHandling = DefaultValueHandling.Include)]
        public string Provider;

        /// <summary>
        /// Set to true if the event occurred while the app was in the foreground state
        /// </summary>
        [JsonProperty("fg", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool? IsForeground;
    }
}
