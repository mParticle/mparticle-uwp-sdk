using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class RequestHeaderSdkMessage : SdkMessage
    {
        /// <summary>
        /// mParticle SDK version. Required.
        /// </summary>
        [JsonProperty("sdk")]
        public string SdkVersion;

        /// <summary>
        /// mParticle Application Key. Required.
        /// </summary>
        [JsonProperty("a")]
        public string AppKey;

        /// <summary>
        /// User attributes. Optional.
        /// </summary>
        [JsonProperty("ua")]
        public IDictionary<string, object> UserAttributes;

        /// <summary>
        /// User attributes. Optional.
        /// </summary>
        [JsonProperty("uad")]
        public IList<string> DeletedUserAttributes;

        /// <summary>
        /// User id's. Optional.
        /// </summary>
        [JsonProperty("ui")]
        public UserIdentity[] UserIdentities;

        /// <summary>
        /// Device information. Required.
        /// </summary>
        [JsonProperty("di")]
        public DeviceInfo DeviceInfo;

        /// <summary>
        /// Application information. Optional.
        /// </summary>
        [JsonProperty("ai")]
        public AppInfo AppInfo;

        /// <summary>
        /// List of events. Optional.
        /// </summary>
        [JsonProperty("msgs")]
        public SdkMessage[] Messages;

        /// <summary>
        /// Custom module settings.  Optional
        /// </summary>
        [JsonProperty("cms", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<int, Dictionary<string, string>> CustomModuleSettings;

        /// <summary>
        /// The length of the SDK's session timeout timer in seconds
        /// </summary>
        [JsonProperty("stl")]
        public int? SessionTimeoutTimerLength;

        /// <summary>
        /// The length of the SDK's upload interval timer in seconds
        /// </summary>
        [JsonProperty("uitl")]
        public int? UploadIntervalTimerLength;

        /// <summary>
        /// The LTV value the SDK has accumulated
        /// </summary>
        [JsonProperty("ltv")]
        public decimal? LtvValue;

        /// <summary>
        /// The mpId of the user.
        /// </summary>
        [JsonProperty("mpid", NullValueHandling = NullValueHandling.Ignore)]
        public long? ClientMpId;

        /// <summary>
        /// Collection of integration attributes by module id.  Optional.
        /// </summary>
        [JsonProperty("ia", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<int, Dictionary<string, string>> IntegrationAttributesByModule;

        /// <summary>
        /// Device application stamp. This is required for all stateful clients.
        /// </summary>
        [JsonProperty("das", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid DeviceApplicationStamp { get; set; }

        public RequestHeaderSdkMessage()
            : base(MessageDataType.RequestHeaderSdkMessage)
        {

        }
    }
}
