using System;
using Windows.ApplicationModel;

namespace mParticle.Sdk.UWP
{
    internal static class ApplicationInfoBuilder
    {
        internal static string GetAppVersion(PackageVersion version)
        {
            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        internal static Core.Dto.Events.AppInfo Build()
        {
            return new Core.Dto.Events.AppInfo()
            {
                Name = Package.Current.DisplayName,
                Version = GetAppVersion(Package.Current.Id.Version),
                PackageName = Package.Current.Id.Name,
                Architecture = Package.Current.Id.Architecture.ToString(),
                ApplicationBuildNumber = Convert.ToString(Package.Current.Id.Version.Build),
                DebugSigning = Package.Current.SignatureKind == PackageSignatureKind.Developer,
            };
        }
    }
}