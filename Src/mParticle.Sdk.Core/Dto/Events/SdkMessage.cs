using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace mParticle.Sdk.Core.Dto.Events
{
    public class SdkMessage
    {
        /// <summary>
        /// Message type tag. Required.
        /// </summary>
        [JsonProperty("dt", Order = -5)]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageDataType MessageDataType;

        /// <summary>
        /// Unique message identifier. Required.
        /// </summary>
        [JsonProperty("id", Order = -4)]
        public string Id;

        /// <summary>
        /// Message creation timestamp in milliseconds since epoch.
        /// </summary>
        [JsonProperty("ct", Order = -3)]
        public long Timestamp;

        /// <summary>
        /// Debug flag. Optional.
        /// </summary>
        [JsonProperty("dbg", Order = -2)]
        public bool? Debug;

        public SdkMessage(MessageDataType messageDataType)
        {
            this.MessageDataType = messageDataType;
            this.Id = Guid.NewGuid().ToString();
        }
    }
}