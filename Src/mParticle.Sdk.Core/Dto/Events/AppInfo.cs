using System.Collections.Generic;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    /// <summary>
    /// Application information
    /// </summary>
    public sealed class AppInfo
    {
        /// <summary>
        /// Application name. Optional.
        /// </summary>
        [JsonProperty("an")]
        public string Name;

        /// <summary>
        /// Application version. Optional.
        /// </summary>
        [JsonProperty("av")]
        public string Version;

        /// <summary>
        /// Package name. Optional.
        /// </summary>
        [JsonProperty("apn")]
        public string PackageName;

        /// <summary>
        /// Installer package name. Optional.
        /// </summary>
        [JsonProperty("ain")]
        public string InstallerPackageName;

        /// <summary>
        /// Timestamp of the first run in milliseconds since epoch. Optional.
        /// </summary>
        [JsonProperty("ict")]
        public long InitialTimestamp;

        /// <summary>
        /// Install referrer string. Optional.
        /// </summary>
        [JsonProperty("ir")]
        public string InstallReferrer;
        
        [JsonProperty("cri")]
        public uint? CryptId;

        [JsonProperty("crs")]
        public uint? CryptSize;

        [JsonProperty("cro")]
        public uint? CryptOff;

        [JsonProperty("bid")]
        public string BuildUuid;

        [JsonProperty("arc")]
        public string Architecture;

        [JsonProperty("pir")]
        public bool? IsPirated;

        [JsonProperty("abn")]
        public string ApplicationBuildNumber;

        [JsonProperty("dbg")]
        public bool? DebugSigning;

        /// <summary>
        /// Deployment target - minimum SDK version that the application needs to run
        /// </summary>
        [JsonProperty("tsv")]
        public string TargetSdkVersion;

        /// <summary>
        /// The application can use features up to and including this SDK version
        /// </summary>
        [JsonProperty("bsv")]
        public string BuiltSdkVersion;

        /// <summary>
        /// When the application was updated in milliseconds since the epoch.
        /// </summary>
        [JsonProperty("ud")]
        public long UpgradeDate;

        /// <summary>
        /// Number of application launches
        /// </summary>
        [JsonProperty("lc")]
        public int? LaunchCount;

        /// <summary>
        /// Number of application launches since the upgrade
        /// </summary>
        [JsonProperty("lcu")]
        public int? LaunchCountSinceUpgrade;

        /// <summary>
        /// The last time the application was used in milliseconds since the epoch.
        /// </summary>
        [JsonProperty("lud")]
        public long? LastUseDate;

        [JsonProperty("tt")]
        public int? PushNotificationRegisteredTypes;

        /// <summary>
        /// Application environment
        /// </summary>
        /// <remarks>
        /// 1 - Development
        /// 2 - Production
        /// </remarks>
        [JsonProperty("env")]
        public int? AppEnvironment;

        /// <summary>
        /// IOS application icon badge number
        /// </summary>
        [JsonProperty("bn")]
        public int? BadgeNumber;

        /// <summary>
        /// 'true' if the first time the user used this app, it was for an install; 'false' for an upgrade; 'null' for versions of the SDK that aren't reporting this feature
        /// </summary>
        [JsonProperty("fi")]
        public bool? FirstSeenIsInstall;

        /// <summary>
        /// Optional app store receipt string used for app install validation
        /// </summary>
        /// <remarks>
        /// This is base 64 encoded for IOS.
        /// </remarks>
        [JsonProperty("asr")]
        public string AppStoreReceipt;

        /// <summary>
        /// Apple App Store Search Ads attribution arguments. Optional
        /// </summary>
        /// <seealso cref="http://searchads.apple.com/help/measure-results#attribution-api"/>
        [JsonProperty("asaa")]
        public Dictionary<string, Dictionary<string, string>> AppleSearchAdsAttributes { get; set; }
    }
}
