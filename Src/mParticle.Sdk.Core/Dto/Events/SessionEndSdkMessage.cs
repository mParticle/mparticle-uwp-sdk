using System.Collections.Generic;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class SessionEndSdkMessage : SdkMessage
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
        /// Event length. Optional.
        /// </summary>
        [JsonProperty("sl")]
        public long SessionLength;

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
        /// Attributes. Optional.
        /// </summary>
        [JsonProperty("attrs")]
        public IDictionary<string, string> Attributes;

        [JsonProperty("cs")]
        public CurrentState CurrentState { get; set; }

        /// <summary>
        /// Total session length, including background time. Optional.
        /// </summary>
        [JsonProperty("slx")]
        public long SessionLengthWithBackgroundTime;

        /// <summary>
        /// When a session spans multiple mpids, this field will be populated when possible from the client.
        /// 
        /// This can occur when a user logs in or out causing their mpid to change while they're within a single session.
        /// </summary>
        [JsonProperty("smpids")]
        public long[] SpanningMpIds;

        public SessionEndSdkMessage()
            : base(MessageDataType.SessionEndSdkMessage)
        {

        }
    }
}
