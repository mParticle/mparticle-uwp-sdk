using mParticle.Sdk.Core.Dto.Identity;

namespace mParticle.Sdk.Core.Dto.Events
{
    public enum UserIdentityType : byte
    {
        Other = IdentityType.Other,
        CustomerId = IdentityType.CustomerId,
        Facebook = IdentityType.Facebook,
        Twitter = IdentityType.Twitter,
        Google = IdentityType.Google,
        Microsoft = IdentityType.Microsoft,
        Yahoo = IdentityType.Yahoo,
        Email = IdentityType.Email,
        FacebookCustomAudienceId = IdentityType.FacebookCustomAudienceId,
    }
}
