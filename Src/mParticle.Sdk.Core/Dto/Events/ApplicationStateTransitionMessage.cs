using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace mParticle.Sdk.Core.Dto.Events
{
    /// <summary>
    /// Application State Transition messages are sent for app initialize, app exit, app foreground, and app background events.
    /// </summary>
    public sealed class ApplicationStateTransitionMessage : SdkMessage
    {
        /// <summary>
        /// Session identifier. Required.
        /// </summary>
        [JsonProperty("sid")]
        public string SessionId;

        [JsonProperty("t")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ApplicationTransitionType ApplicationTransitionType;

        [JsonProperty("dct")]
        public string DataConnectionType;

        [JsonProperty("lc")]
        public Location Location { get; set; }

        [JsonProperty("cs")]
        public CurrentState CurrentState { get; set; }

        /// <summary>
        /// Push notification message data in json format
        /// </summary>
        [JsonProperty("pay")]
        public string JsonPayload { get; set; }
        
        [JsonProperty("ifr")]
        public bool IsFirstRun { get; set; }

        [JsonProperty("iu")]
        public bool IsUpgrade { get; set; }

        /// <summary>
        /// Set to true if previous session successfully closed
        /// </summary>
        [JsonProperty("sc")]
        public bool SuccessfullyClosed;

        /// <summary>
        /// View controller.  IOS only, optional
        /// </summary>
        [JsonProperty("vc")]
        public string ViewController { get; set; }

        /// <summary>
        /// Whether the AppEvent call is occurring on the main thread.  IOS only, optional.
        /// </summary>
        /// <remarks>
        /// Should return the value of [NSThread isMainThread]
        /// </remarks>
        [JsonProperty("mt")]
        public bool? IsMainThread { get; set; }

        /// <summary>
        /// Activity canonical name.  Android only, optional.
        /// </summary>
        [JsonProperty("cn")]
        public string CanonicalName { get; set; }

        /// <summary>
        /// LaunchReferral. Optional.
        /// </summary>
        [JsonProperty("lr")]
        public string LaunchReferral;

        /// <summary>
        /// The referral application
        /// </summary>
        /// <example>
        /// e.g. com.companyABC.AppName
        /// </example>
        [JsonProperty("src")]
        public string ReferralApplication;

        /// <summary>
        /// Application launch parameters
        /// </summary>
        [JsonProperty("lpr")]
        public string LaunchParameters;

        /// <summary>
        /// Whether the session was finalized
        /// </summary>
        [JsonProperty("sf")]
        public bool? SessionFinalized;

        /// <summary>
        /// Source package (Android only)
        /// </summary>
        [JsonProperty("srp")]
        public string SourcePackage;

        /// <summary>
        /// The number of session interruptions
        /// </summary>
        [JsonProperty("nsi")]
        public int NumSessionInterruptions;

        /// <summary>
        /// Time elapsed since the session was suspended
        /// </summary>
        [JsonProperty("tls")]
        public long TimeSinceSuspended;

        /// <summary>
        /// The previous foreground time in milliseconds
        /// </summary>
        [JsonProperty("pft")]
        public long PreviousForegroundTime;

        public ApplicationStateTransitionMessage()
            : base(MessageDataType.ApplicationStateTransitionMessage)
        {
        }
    }
}
