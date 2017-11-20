using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Identity
{
    public class Error
    {
        /// <summary>
        /// Code of error.
        /// </summary>
        [JsonProperty("code")]
        public ErrorCode Code { get; set; }

        /// <summary>
        /// A message indicating the error.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}