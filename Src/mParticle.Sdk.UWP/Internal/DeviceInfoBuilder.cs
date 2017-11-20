using System;
using System.Globalization;
using mParticle.Sdk.Core.Dto.Events;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;
using Windows.System.UserProfile;

namespace mParticle.Sdk.UWP
{
    internal static class DeviceInfoBuilder
    {
        internal static DeviceInfo Build()
        {
            var easClientDeviceInformation = new EasClientDeviceInformation();
            var displayInformation = DisplayInformation.GetForCurrentView();
            var cultureInfo = new CultureInfo(GlobalizationPreferences.Languages[0].ToString());

            return new DeviceInfo()
            {
                MicrosoftAdvertisingId = QueryAdvertisingId(),
                MicrosoftPublisherId = QueryPublisherId(),
                Platform = "xbox",
                Brand = easClientDeviceInformation.SystemManufacturer,
                Product = easClientDeviceInformation.SystemProductName,
                Name = easClientDeviceInformation.FriendlyName,
                Manufacturer = easClientDeviceInformation.SystemManufacturer,
                Model = easClientDeviceInformation.SystemSku,
                ScreenWidth = (int)displayInformation.ScreenWidthInRawPixels,
                ScreenHeight = (int)displayInformation.ScreenHeightInRawPixels,
                ScreenDpi = (int)displayInformation.LogicalDpi,
                LocaleLanguage = cultureInfo.TwoLetterISOLanguageName,
            };
        }

        public static string QueryAdvertisingId()
        {
            return AdvertisingManager.AdvertisingId;
        }

        public static string QueryPublisherId()
        {
            string publisherId = null;
            if (ApiInformation.IsTypePresent("Windows.System.Profile.SystemIdentification"))
            {
                var systemId = SystemIdentification.GetSystemIdForPublisher();
                if (systemId.Source != SystemIdentificationSource.None)
                {
                    var idBuffer = systemId.Id;
                    var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(idBuffer);
                    var bytes = new byte[idBuffer.Length];
                    dataReader.ReadBytes(bytes);
                    publisherId = Convert.ToBase64String(bytes);
                }
            }
            return publisherId;
        }
    }
}