using Newtonsoft.Json;
using System.Collections.Generic;

namespace mParticle.Sdk.Core.Dto.Identity
{
    public class ModifyRequest : IdentityBaseRequest
    {
        [JsonProperty("identity_changes")]
        public IEnumerable<IdentityChange> IdentityChanges { get; set; }
    }
}