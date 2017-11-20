using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class UserIdentity
    {
        /// <summary>
        /// Identity value. Required.
        /// </summary>
        [JsonProperty("i")]
        public string Identity;

        /// <summary>
        /// Identity tag. Required.
        /// </summary>
        [JsonProperty("n")]
        public UserIdentityType Name;

        /// <summary>
        /// The date this identity was first set.  Represented in milliseconds since the epoch. Required.
        /// </summary>
        [JsonProperty("dfs")]
        public long DateFirstSet;

        /// <summary>
        /// Whether this was the first time we've seen this identity. Required.
        /// </summary>
        [JsonProperty("f")]
        public bool IsFirstSeen;
    }
}