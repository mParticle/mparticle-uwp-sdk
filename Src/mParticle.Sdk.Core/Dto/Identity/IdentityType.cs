using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace mParticle.Sdk.Core.Dto.Identity
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IdentityType : byte
    {
        [EnumMember(Value = "other")]
        Other = 0,

        [EnumMember(Value = "customerid")]
        CustomerId = 1,

        [EnumMember(Value = "facebook")]
        Facebook = 2,

        [EnumMember(Value = "twitter")]
        Twitter = 3,

        [EnumMember(Value = "google")]
        Google = 4,

        [EnumMember(Value = "microsoft")]
        Microsoft = 5,

        [EnumMember(Value = "yahoo")]
        Yahoo = 6,

        [EnumMember(Value = "email")]
        Email = 7,

        [EnumMember(Value = "facebook_custom_audience_id")]
        FacebookCustomAudienceId = 9,

        [EnumMember(Value="roku_aid")]
        RokuAid = 10,

        [EnumMember(Value = "android_aaid")]
        AndroidAaid = 11,

        [EnumMember(Value = "ios_idfa")]
        IosIdfa = 12,

        [EnumMember(Value = "amp_id")]
        AmpId = 13,

        [EnumMember(Value = "android_uuid")]
        AndroidUuid = 14,

        [EnumMember(Value = "ios_idfv")]
        IosIdfv = 15,

        [EnumMember(Value = "push_token")]
        PushToken = 16,

        [EnumMember(Value = "roku_publisher_id")]
        RokuPublisherId = 17,

        [EnumMember(Value = "device_application_stamp")]
        DeviceApplicationStamp = 18,

        [EnumMember(Value = "microsoft_aid")]
        MicrosoftAdvertisingId = 19,

        [EnumMember(Value = "microsoft_publisher_id")]
        MicrosoftPublisherId = 20
    }
}