using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Identity
{
    public class IdentityChange
    {
        [JsonProperty("old_value")]
        public string OldValue { get; set;}

        [JsonProperty("new_value")]
        public string NewValue { get; set;}

        [JsonProperty("identity_type")]
        public IdentityType IdentityType { get; set; }
    }
}