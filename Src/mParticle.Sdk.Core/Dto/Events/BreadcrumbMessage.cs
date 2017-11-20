using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class BreadcrumbMessage : SdkMessage
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
        /// Session number of breadcrumb leading up to crash
        /// </summary>
        [JsonProperty("sn")]
        public int SessionNumber;

        /// <summary>
        /// Breadcrumb label
        /// </summary>
        [JsonProperty("l")]
        public string Label;

        public BreadcrumbMessage()
            : base(MessageDataType.BreadcrumbMessage)
        {
        }
    }
}
