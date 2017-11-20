using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class OptOutSdkMessage : SdkMessage
    {
        /// <summary>
        /// Session identifier. Optional.
        /// </summary>
        [JsonProperty("sid")]
        public string SessionId;

        /// <summary>
        /// Session timestamp. Optional.
        /// </summary>
        [JsonProperty("sct")]
        public long SessionStartTimestamp;

        /// <summary>
        /// Opt-out/in flag. Required.
        /// </summary>
        [JsonProperty("s", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool OptOut;

        public OptOutSdkMessage()
            : base(MessageDataType.OptOutSdkMessage)
        {
            
        }
    }
}
