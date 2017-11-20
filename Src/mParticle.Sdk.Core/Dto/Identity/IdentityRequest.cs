using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Identity
{
    public class IdentityRequest : IdentityBaseRequest
    {
        [JsonProperty("known_identities")]
        public Identities KnownIdentities { get; set; }
    }
}