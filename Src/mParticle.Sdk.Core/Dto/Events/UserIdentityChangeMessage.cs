using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class UserIdentityChangeMessage : SdkMessage
    {
        [JsonProperty("sid")]
        public string SessionId { get; set; }

        [JsonProperty("cs")]
        public CurrentState CurrentState { get; set; }

        /// <summary>
        /// The new user identity.
        /// 
        /// If BOTH the new and old user identities are NOT null, it represents a change to the user identity;
        /// If the new user identity is NOT null AND the old user identity IS null, it represents a NEW user identity;
        /// If the new user identity IS NULL AND the old user identity is NOT null, it represents a deleted user identity;
        /// 
        /// If BOTH the new and old user identities are null, it indicates a data problem;
        /// </summary>
        [JsonProperty("ni")]
        public UserIdentity NewUserIdentity { get; set; }

        /// <summary>
        /// The old user identity.
        /// 
        /// If BOTH the new and old user identities are NOT null, it represents a change to the user identity;
        /// If the new user identity is NOT null AND the old user identity IS null, it represents a NEW user identity;
        /// If the new user identity IS NULL AND the old user identity is NOT null, it represents a deleted user identity;
        /// 
        /// If BOTH the new and old user identities are null, it indicates a data problem;
        /// </summary>
        [JsonProperty("oi")]
        public UserIdentity OldUserIdentity { get; set; }

        public UserIdentityChangeMessage() : base(MessageDataType.UserIdentityChangeMessage)
        {
        }
    }
}
