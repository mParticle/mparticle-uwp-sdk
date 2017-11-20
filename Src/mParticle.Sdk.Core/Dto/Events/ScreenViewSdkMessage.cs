using System.Collections.Generic;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class ScreenViewSdkMessage : SdkMessage
    {
        /// <summary>
        /// Session identifier. Required.
        /// </summary>
        [JsonProperty("sid")]
        public string SessionId;

        /// <summary>
        /// Session start timestamp. Required.
        /// </summary>
        [JsonProperty("sct")]
        public long SessionStartTimestamp;

        /// <summary>
        /// Event name. Required.
        /// </summary>
        [JsonProperty("n")]
        public string ScreenName;

        /// <summary>
        /// Event timestamp. Required.
        /// </summary>
        [JsonProperty("est")]
        public long EventStartTimestamp;

        /// <summary>
        /// Event length. Optional.
        /// </summary>
        [JsonProperty("els")]
        public long EventLength;

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
        
        [JsonProperty("cs")]
        public CurrentState CurrentState { get; set; }

        /// <summary>
        /// Activity type.  Optional - only used for Android
        /// </summary>
        [JsonProperty("t")]
        public string ActivityType { get; set; }

        /// <summary>
        /// Attributes. Optional.
        /// </summary>
        [JsonProperty("attrs")]
        public IDictionary<string, string> Attributes;

        /// <summary>
        /// Additional attributes for screen view events that will not be forwarded by default
        /// </summary>
        [JsonProperty("flags", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, string> CustomFlags { get; set; }

        public ScreenViewSdkMessage()
            : base(MessageDataType.ScreenViewSdkMessage)
        {

        }
    }
}
