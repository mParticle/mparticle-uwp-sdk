using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace mParticle.Sdk.Core.Dto.Identity
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Platform
    {
        [EnumMember(Value = "android")]
        Android,
        [EnumMember(Value = "ios")]
        Ios,
        [EnumMember(Value = "tvos")]
        Tvos,
        [EnumMember(Value = "roku")]
        Roku,
        [EnumMember(Value = "web")]
        Web,
        [EnumMember(Value = "alexa")]
        Alexa,
        [EnumMember(Value = "fire")]
        Fire,
        [EnumMember(Value = "xbox")]
        Xbox,
        [EnumMember(Value = "smart_tv")]
        SmartTV
    }
}