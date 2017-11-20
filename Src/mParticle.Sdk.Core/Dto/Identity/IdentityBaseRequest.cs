using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace mParticle.Sdk.Core.Dto.Identity
{
    public abstract class IdentityBaseRequest
    {
        [JsonProperty("environment")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Environment? Environment { get; set; }

        [JsonProperty("client_sdk")]
        public ClientSdk ClientSdk { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("source_request_id")]
        public string SourceRequestId { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
        
        [JsonProperty("request_timestamp_ms")]
        public long? RequestTimestampMs { get; set; }
    }
}