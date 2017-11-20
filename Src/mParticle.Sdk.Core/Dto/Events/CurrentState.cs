using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    /// <summary>
    /// This structure contains all meaningful state information at the time an event occurred.
    /// </summary>
    public sealed class CurrentState
    {
        [JsonProperty("cpu", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Cpu { get; set; }

        [JsonProperty("sma", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double SystemMemoryAvailable { get; set; }

        [JsonProperty("sml", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? SystemMemoryLow { get; set; }

        [JsonProperty("smt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double SystemMemoryThreshold { get; set; }

        [JsonProperty("ama", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double AppMemoryAvail { get; set; }

        [JsonProperty("amm", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double AppMemoryMax { get; set; }

        [JsonProperty("amt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double AppMemoryTotal { get; set; }

        [JsonProperty("so", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string DeviceOrientation;	

        [JsonProperty("sbo", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StatusBarOrientation { get; set; }

        [JsonProperty("tss", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long TimeSinceStartInMilliseconds;

        [JsonProperty("bl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double BatteryLevel;

        [JsonProperty("dct", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string DataConnectionType;

        [JsonProperty("gps", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? GpsState;

        [JsonProperty("tsm", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long TotalSystemMemoryUsage;

        [JsonProperty("fds", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long DiskSpaceFree;

        [JsonProperty("efds", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long ExternalDiskSpaceFree;

        /// <summary>
        /// Android network type
        /// </summary>
        /// <remarks>
        /// Value comes from TelephonyManager object <see cref="http://developer.android.com/reference/android/telephony/TelephonyManager.html"/>
        /// Default value of 0 is NETWORK_TYPE_UNKNOWN
        /// </remarks>
        [JsonProperty("ant", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int NetworkType;
    }
}
