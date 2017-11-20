using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace mParticle.Sdk.Core.Dto.Identity
{
    public class ErrorResponse : ResponseBase
    {
        [JsonProperty("errors")]
        public IEnumerable<Error> Errors { get; set; }

        [JsonProperty("errorCode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorCode ErrorCode { get; private set; }

        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
    }
}