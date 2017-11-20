using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Identity
{
    public class IdentityResponse : ResponseBase
    {
        /// <summary>
        /// Base 64 string to store for the subsequent request.
        /// </summary>
        [JsonProperty("context")]
        public string Context { get; set; }
        
        [JsonProperty("matched_identities")]
        public Identities MatchedIdentities { get; set; }

        /// <summary>
        /// If true, the ID and its storage can be deleted by the client upon change of ID.
        /// </summary>
        /// <value>If true, the ID and its storage can be deleted by the client upon change of ID.</value>
        [JsonProperty("is_ephemeral")]
        public bool? IsEphemeral { get; private set; }
        /// <summary>
        /// The mParticle ID associated with this Identity.
        /// </summary>
        /// <value>The mParticle ID associated with this Identity.</value>
        [JsonProperty("mpid")]
        public string Mpid { get; set; }
    }
}