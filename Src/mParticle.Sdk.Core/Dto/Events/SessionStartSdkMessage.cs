using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class SessionStartSdkMessage : SdkMessage
    {
        /// <summary>
        /// Location. Optional.
        /// </summary>
        [JsonProperty("lc")]
        public Location Location;

        /// <summary>
        /// Data connection type. Required.
        /// </summary>
        [JsonProperty("dct")]
        public string DataConnectionType;

        /// <summary>
        /// LaunchReferral. Optional.
        /// </summary>
        [JsonProperty("lr")]
        public string LaunchReferral;

        [JsonProperty("cs")]
        public CurrentState CurrentState { get; set; }

        /// <summary>
        /// Previous session length in seconds
        /// </summary>
        [JsonProperty("psl")]
        public long PreviousSessionLength;

        /// <summary>
        /// Previous session identifier
        /// </summary>
        [JsonProperty("pid")]
        public string PreviousSessionId;

        /// <summary>
        /// Previous session start time in milliseconds since the Unix epoch.  Optional.
        /// </summary>
        [JsonProperty("pss")]
        public long PreviousSessionStartTime;

        public SessionStartSdkMessage()
            : base(MessageDataType.SessionStartSdkMessage)
        {

        }
    }
}
