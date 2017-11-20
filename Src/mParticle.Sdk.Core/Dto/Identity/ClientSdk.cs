using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Identity
{
    public class ClientSdk
    {
        [JsonProperty("platform")]
        public Platform? Platform { get; set; }

        [JsonProperty("sdk_vendor")]
        public string SdkVendor { get; set; }

        [JsonProperty("sdk_version")]
        public string SdkVersion { get; set; }
    }
}