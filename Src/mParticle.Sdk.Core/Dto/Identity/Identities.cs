using Newtonsoft.Json;
using System.Collections.Generic;

namespace mParticle.Sdk.Core.Dto.Identity
{
    [JsonConverter(typeof(IdentityTypeDictionaryConverter))]
    public class Identities : Dictionary<IdentityType, string>
    {
      
    }
}