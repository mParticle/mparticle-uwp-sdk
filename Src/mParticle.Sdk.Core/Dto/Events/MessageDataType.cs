using System.Runtime.Serialization;

namespace mParticle.Sdk.Core.Dto.Events
{
    public enum MessageDataType
    {
        Undefined = 0,

        [EnumMember(Value = "h")]
        RequestHeaderSdkMessage,

        [EnumMember(Value = "ss")]
        SessionStartSdkMessage,

        [EnumMember(Value = "se")]
        SessionEndSdkMessage,

        [EnumMember(Value = "e")]
        EventSdkMessage,

        [EnumMember(Value = "v")]
        ScreenViewSdkMessage,

        [EnumMember(Value = "o")]
        OptOutSdkMessage,

        [EnumMember(Value = "x")]
        CrashReportSdkMessage,

        [EnumMember(Value = "rh")]
        ResponseHeaderSdkMessage,

        [EnumMember(Value = "hc")]
        HttpCommandSdkMessage,

        [EnumMember(Value = "ac")]
        AppConfigSdkMessage,

        [EnumMember(Value = "pr")]
        PushRegistrationSdkMessage,

        [EnumMember(Value = "fr")]
        FirstRunMessage,

        [EnumMember(Value = "ast")]
        ApplicationStateTransitionMessage,

        [EnumMember(Value = "pm")]
        PushMessageMessage,

        [EnumMember(Value = "npe")]
        NetworkPerformanceEventMessage,

        [EnumMember(Value = "bc")]
        BreadcrumbMessage,

        [EnumMember(Value = "ar")]
        AudienceResponseMessage,

        [EnumMember(Value = "pro")]
        ProfileMessage,

        [EnumMember(Value = "pre")]
        PushReaction,

        [EnumMember(Value = "cm")]
        CommerceSdkMessage,

        [EnumMember(Value = "uac")]
        UserAttributeChangeMessage,

        [EnumMember(Value = "uic")]
        UserIdentityChangeMessage,

        [EnumMember(Value = "un")]
        Uninstall,
    }
}
