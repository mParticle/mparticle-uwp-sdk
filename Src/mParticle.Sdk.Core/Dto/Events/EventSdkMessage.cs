using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class EventSdkMessage : SdkMessage
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
        public string EventName;

        /// <summary>
        /// Event type. Required.
        /// </summary>
        [JsonProperty("et")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EventType EventType;

        /// <summary>
        /// Event timestamp. Required.
        /// </summary>
        [JsonProperty("est")]
        public long EventStartTimestamp;

        /// <summary>
        /// Event length. Optional.
        /// </summary>
        [JsonProperty("el")]
        public long? EventLength;

        /// <summary>
        /// Attributes. Optional.
        /// </summary>
        [JsonProperty("attrs")]
        public IDictionary<string, string> Attributes;

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

        [JsonProperty("en")]
        public int EventNum { get; set; }

        /// <summary>
        /// View controller.  IOS only, optional
        /// </summary>
        [JsonProperty("vc")]
        public string ViewController { get; set; }

        /// <summary>
        /// Whether the AppEvent call is occurring on the main thread.  IOS only, optional.
        /// </summary>
        /// <remarks>
        /// Should return the value of [NSThread isMainThread]
        /// </remarks>
        [JsonProperty("mt")]
        public bool? IsMainThread { get; set; }

        /// <summary>
        /// Activity canonical name.  Android only, optional.
        /// </summary>
        [JsonProperty("cn")]
        public string CanonicalName { get; set; }

        /// <summary>
        /// Additional attributes for events that will not be forwarded by default
        /// </summary>
        [JsonProperty("flags", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, List<string>> CustomFlags { get; set; }

        public EventSdkMessage()
            : base(MessageDataType.EventSdkMessage)
        {
        }
    }
}
