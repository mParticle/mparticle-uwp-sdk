using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class UserAttributeChangeMessage : SdkMessage
    {
        [JsonProperty("sid")]
        public string SessionId { get; set; }

        [JsonProperty("cs")]
        public CurrentState CurrentState { get; set; }

        /// <summary>
        /// The name of the User Attribute, required cannot be null, empty, or whitespace.
        /// </summary>
        [JsonProperty("n")]
        public string UserAttributeName { get; set; }

        /// <summary>
        /// The newly applied value for the user attribute.
        /// 
        /// If the user attribute was deleted, this value should be null.
        /// 
        /// However, since the new value may be a tag, the new value may be null while NOT representing a delete.
        /// 
        /// This may either be:
        /// 
        /// 1) a string;
        /// 2) an array of strings.
        /// </summary>
        [JsonProperty("nv")]
        public object NewValue { get; set; }

        /// <summary>
        /// The old value for the user attribute.
        /// 
        /// If this is a new user attribute, this value should be null.
        /// 
        /// This may either be:
        /// 
        /// 1) a string;
        /// 2) an array of strings.
        /// </summary>
        [JsonProperty("ov")]
        public object OldValue { get; set; }

        /// <summary>
        /// If the user attribute was deleted.
        /// </summary>
        [JsonProperty("d")]
        public bool Deleted { get; set; }

        /// <summary>
        /// Determines if this change represents adding a new user attribute.
        /// </summary>
        [JsonProperty("na")]
        public bool IsNewAttribute { get; set; }

        public UserAttributeChangeMessage() 
            : base(MessageDataType.UserAttributeChangeMessage)
        {
        }
    }
}
