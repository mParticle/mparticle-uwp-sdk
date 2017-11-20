using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events.Commerce
{
    public sealed class Promotion
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("nm")]
        public string Name;

        [JsonProperty("cr")]
        public string Creative;

        [JsonProperty("ps")]
        public string Position;
    }
}
