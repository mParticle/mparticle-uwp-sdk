using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class LocationTracking
    {
        /// <summary>
        /// Specifies the location tracking mode for the app.
        /// </summary>
        [JsonProperty("ltm", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public LocationTrackingMode LocationTrackingMode;

        /// <summary>
        /// How accurate in meters the location should be
        /// </summary>
        [JsonProperty("acc", NullValueHandling = NullValueHandling.Ignore)]
        public int Accuracy;

        /// <summary>
        /// How far the device has to move (in meters) to trigger a location refresh
        /// </summary>
        [JsonProperty("mdf", NullValueHandling = NullValueHandling.Ignore)]
        public int MinDistanceFilter;
    }
}
