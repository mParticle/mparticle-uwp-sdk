using System.Runtime.Serialization;

namespace mParticle.Sdk.Core.Dto.Identity
{
    public enum Environment
    {
        [EnumMember(Value = "production")]
        Production,
        
        [EnumMember(Value = "development")]
        Development
    }
}