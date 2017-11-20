using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class FirstRunMessage : SdkMessage
    {
        /// <summary>
        /// Data connection type. Required.
        /// </summary>
        [JsonProperty("dct")]
        public string DataConnectionType;

        /// <summary>
        /// Location. Optional.
        /// </summary>
        [JsonProperty("lc")]
        public Location Location;

        public FirstRunMessage()
            : base(MessageDataType.FirstRunMessage)
        {

        }
    }
}