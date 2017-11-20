using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class DeviceInfo
    {
        /// <summary>
        /// Brand. Optional.
        /// </summary>
        [JsonProperty("b")]
        public string Brand;

        /// <summary>
        /// Product. Optional.
        /// </summary>
        [JsonProperty("p")]
        public string Product;

        /// <summary>
        /// Name. Optional.
        /// </summary>
        [JsonProperty("dn")]
        public string Name;

        /// <summary>
        /// Udid. Required.
        /// </summary>
        [JsonProperty("duid")]
        public string Udid;

        /// <summary>
        /// WifiMac. Optional.
        /// </summary>
        [JsonProperty("wmac")]
        public string WifiMac;

        /// <summary>
        /// EthernetMac. Optional.
        /// </summary>
        [JsonProperty("emac")]
        public string EthernetMac;

        /// <summary>
        /// BluetoothMac. Optional.
        /// </summary>
        [JsonProperty("bmac")]
        public string BluetoothMac;

        /// <summary>
        /// Imei. Optional.
        /// </summary>
        [JsonProperty("imei")]
        public string Imei;

        /// <summary>
        /// Anid. Optional.
        /// </summary>
        [JsonProperty("anid")]
        public string Anid;

        /// <summary>
        /// Manufacturer. Optional.
        /// </summary>
        [JsonProperty("dma")]
        public string Manufacturer;

        /// <summary>
        /// Platform. Required
        /// </summary>
        [JsonProperty("dp")]
        public string Platform;

        /// <summary>
        /// OsVersion. Optional
        /// </summary>
        [JsonProperty("dosv")]
        public string OsVersion;

        /// <summary>
        /// Model. Optional
        /// </summary>
        [JsonProperty("dmdl")]
        public string Model;

        /// <summary>
        /// ScreenHeight. Optional
        /// </summary>
        [JsonProperty("dsh")]
        public int? ScreenHeight;

        /// <summary>
        /// ScreenWidth. Optional
        /// </summary>
        [JsonProperty("dsw")]
        public int? ScreenWidth;

        /// <summary>
        /// ScreenDpi. Optional
        /// </summary>
        [JsonProperty("dpi")]
        public int? ScreenDpi;

        /// <summary>
        /// Device Country. Optional
        /// </summary>
        [JsonProperty("dc")]
        public string Country;

        /// <summary>
        /// LocaleLanguage. Optional
        /// </summary>
        [JsonProperty("dll")]
        public string LocaleLanguage;

        /// <summary>
        /// LocaleCountry. Optional
        /// </summary>
        [JsonProperty("dlc")]
        public string LocaleCountry;

        /// <summary>
        /// NetworkCountry. Optional
        /// </summary>
        [JsonProperty("nc")]
        public string NetworkCountry;

        /// <summary>
        /// NetworkCarrier. Optional
        /// </summary>
        [JsonProperty("nca")]
        public string NetworkCarrier;

        /// <summary>
        /// MobileNetworkCode. Optional
        /// </summary>
        [JsonProperty("mnc")]
        public string MobileNetworkCode;

        /// <summary>
        /// MobileCountryCode. Optional.
        /// </summary>
        [JsonProperty("mcc")]
        public string MobileCountryCode;

        /// <summary>
        /// UtcOffset. Optional.
        /// </summary>
        [JsonProperty("tz")]
        public int? UtcOffset;

        /// <summary>
        /// BuildId. Optional.
        /// </summary>
        [JsonProperty("bid")]
        public string BuildId;

        /// <summary>
        /// AdvertisingIdentifier from IOS. Optional.
        /// </summary>
        [JsonProperty("aid")]
        public Guid? AdvertisingIdentifier;

        /// <summary>
        /// The Device Identification for push notifications.
        /// </summary>
        [JsonProperty("to")]
        public string RegistrationToken;

        [JsonProperty("tot", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public PushServiceType? PushServiceType;

        [JsonProperty("ouid")]
        public string OpenUdid;

        [JsonProperty("arc")]
        public string Architecture;

        [JsonProperty("jb")]
        public Dictionary<string, string> Jailbroken;

        /// <summary>
        /// The name of the time zone, e.g.: America/New_York.  Optional.
        /// </summary>
        [JsonProperty("tzn")]
        public string TimeZoneName;

        /// <summary>
        /// 'true' if the device is a tablet
        /// </summary>
        [JsonProperty("it")]
        public bool? IsTablet;

        /// <summary>
        /// Whether sound is enabled on the push registration.  Optional.
        /// </summary>
        [JsonProperty("se")]
        public bool? SoundEnabled;

        /// <summary>
        /// Whether vibrate is enabled on the push registration.  Optional.
        /// </summary>
        [JsonProperty("ve")]
        public bool? VibrateEnabled;

        /// <summary>
        /// The current radio access technology.  Optional. 
        /// </summary>
        [JsonProperty("dr")]
        public string RadioAccessTechnology;

        /// <summary>
        /// Whether the device supports telephony.  Optional.
        /// </summary>
        [JsonProperty("dst")]
        public bool? SupportsTelephony;

        /// <summary>
        /// Whether the device supports NFC (Near Field Communication).  Optional.
        /// </summary>
        [JsonProperty("dsnfc")]
        public bool? HasNFC;

        /// <summary>
        /// Whether the device has Bluetooth enabled.  Optional.
        /// </summary>
        [JsonProperty("dbe")]
        public bool? BluetoothEnabled;

        /// <summary>
        /// Bluetooth version.  Optional.
        /// </summary>
        [JsonProperty("dbv")]
        public string BluetoothVersion;

        /// <summary>
        /// The user visible SDK version.  Optional
        /// </summary>
        [JsonProperty("dosvi")]
        public int? OsVersionInt;

        /// <summary>
        /// IDFV for iOS.  Optional
        /// </summary>
        [JsonProperty("vid")]
        public Guid? VendorIdentifier;

        /// <summary>
        /// Google's advertising ID.  Optional.
        /// </summary>
        /// <seealso cref="http://developer.android.com/google/play-services/id.html" />
        [JsonProperty("gaid")]
        public Guid? GoogleAdvertisingIdentifier;

        [JsonProperty("vr")]
        public string VersionRelease;

        /// <summary>
        /// Whether LimitAdTracking is set on the device
        /// </summary>
        /// <remarks>
        /// This feature is available as of IOS 6, in which case it will contain a null 
        /// </remarks>
        [JsonProperty("lat")]
        public bool? LimitAdTracking;

        /// <summary>
        /// Whether the device is operating in daylight saving time
        /// </summary>
        [JsonProperty("idst")]
        public bool? IsDaylightSavingTime;

        /// <summary>
        /// The Roku Advertising Identifier
        /// </summary>
        /// <see cref="https://sdkdocs.roku.com/display/sdkdoc/ifDeviceInfo#ifDeviceInfo-GetAdvertisingId()asString" />
        [JsonProperty("rida")]
        public Guid? RokuAdvertisingIdentifier;

        /// <summary>
        /// A unique identifier of the unit running the script
        /// </summary>
        /// <see cref="https://sdkdocs.roku.com/display/sdkdoc/ifDeviceInfo#ifDeviceInfo-GetPublisherId()asString" /> 
        [JsonProperty("rpb")]
        public string RokuPublisherId;

        /// <summary>
        /// The Microsoft Advertising Id for UWP apps, from Windows.System.UserProfile.AdvertisingManager
        /// </summary>
        /// <see cref="https://docs.microsoft.com/en-us/uwp/api/windows.system.userprofile.advertisingmanager" />
        [JsonProperty("msaid")]
        public string MicrosoftAdvertisingId;

        /// <summary>
        /// The Microsoft Publisher Id for UWP apps
        /// </summary>
        /// <see cref="https://docs.microsoft.com/en-us/uwp/api/windows.system.profile.systemidentification#Windows_System_Profile_SystemIdentification_GetSystemIdForPublisher" />
        [JsonProperty("mspid")]
        public string MicrosoftPublisherId;
    }
}
