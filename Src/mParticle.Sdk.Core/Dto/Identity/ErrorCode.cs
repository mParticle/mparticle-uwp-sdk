using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace mParticle.Sdk.Core.Dto.Identity
{
    [JsonConverter(typeof(StringEnumConverter))] 
    public enum ErrorCode
    {
        [EnumMember(Value = "INTERNAL_ERROR")]
        Unknown = 0,
        [EnumMember(Value = "VALIDATION_ERROR")]
        ValidationError,
        [EnumMember(Value = "LOOKUP_ERROR")]
        LookupError,
        [EnumMember(Value = "ALIAS_ERROR")]
        AliasError,
    }
}