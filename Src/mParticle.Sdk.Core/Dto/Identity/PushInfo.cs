using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Identity
{
    public class PushInfo
    {
        [JsonProperty("platform")]
        public Platform? Platform { get; set; }

        [JsonProperty("token")]
        public string Token { get; set;}
    }
}