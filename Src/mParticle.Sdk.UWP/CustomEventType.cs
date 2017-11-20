using System.Runtime.Serialization;

namespace mParticle.Sdk.UWP
{
    public enum CustomEventType : byte
    {
        [EnumMember(Value = "navigation")]
        Navigation = 1,

        [EnumMember(Value = "location")]
        Location = 2,

        [EnumMember(Value = "search")]
        Search = 3,

        [EnumMember(Value = "transaction")]
        Transaction = 4,

        [EnumMember(Value = "user_content")]
        UserContent = 5,

        [EnumMember(Value = "user_preference")]
        UserPreference = 6,

        [EnumMember(Value = "social")]
        Social = 7,

        [EnumMember(Value = "other")]
        Other = 8
    }
}