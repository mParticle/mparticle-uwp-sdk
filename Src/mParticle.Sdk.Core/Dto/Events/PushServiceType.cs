using System.Runtime.Serialization;

namespace mParticle.Sdk.Core.Dto.Events
{
    public enum PushServiceType : byte
    {
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        [EnumMember(Value = "appleSandbox")]
        AppleSandbox,

        [EnumMember(Value = "appleProduction")]
        AppleProduction,

        [EnumMember(Value = "google")]
        Google,
    }
}