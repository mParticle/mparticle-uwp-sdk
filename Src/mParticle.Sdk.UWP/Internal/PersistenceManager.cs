using System;
using System.Collections.Generic;
using System.Linq;
using mParticle.Sdk.Core.Dto.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.ApplicationModel;
using Windows.Storage;


namespace mParticle.Sdk.UWP
{
    internal sealed class PersistenceManager
    {
        private readonly MParticleOptions options;
        private ApplicationDataContainer dataContainer;

        public bool IsFirstRun { get; private set; }
        public bool IsUpgrade { get; private set; }

        public PersistenceManager(MParticleOptions options)
        {
            this.options = options;
        }

        public void Initialize(PackageVersion packageVersion)
        {
            dataContainer =
   ApplicationData.Current.LocalSettings.CreateContainer(options.DataContainer, ApplicationDataCreateDisposition.Always);

            IsFirstRun = determineIsFirstRun(dataContainer);
            if (IsFirstRun)
            {
                FirstRunTimeMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }

            IsUpgrade = determineIsUpgrade(dataContainer, packageVersion);
            LastApplicationVersion = ApplicationInfoBuilder.GetAppVersion(packageVersion);
        }

        public long? CurrentMpid
        {
            get
            {
                return (long?)dataContainer.Values[StorageKeys.CurrentMpid];
            }
            set
            {
                dataContainer.Values[StorageKeys.CurrentMpid] = value;
            }
        }

        public long? FirstRunTimeMillis
        {
            get
            {
                return (long?)dataContainer.Values[StorageKeys.FirstRunTime];
            }
            private set
            {
                dataContainer.Values[StorageKeys.FirstRunTime] = value;
            }
        }

        public string LastApplicationVersion
        {
            get
            {
                return (string)dataContainer.Values[StorageKeys.ApplicationVersion];
            }
            set
            {
                dataContainer.Values[StorageKeys.ApplicationVersion] = value;
            }
        }

        private bool determineIsFirstRun(ApplicationDataContainer container)
        {
            return !(container.Values.ContainsKey(StorageKeys.FirstRunTime));
        }

        private bool determineIsUpgrade(ApplicationDataContainer container, PackageVersion packageVersion)
        {
            var persistedAppVersion = LastApplicationVersion;
            if (persistedAppVersion == null)
                return false;

            var currentAppVersion = ApplicationInfoBuilder.GetAppVersion(packageVersion);
            return !(currentAppVersion.Equals(persistedAppVersion));
        }

        public void Clear()
        {
            dataContainer.Values.Clear();
        }

        public bool IsOptOut
        {
            get
            {
                return (bool?)dataContainer.Values[StorageKeys.IsOptOut] ?? false;
            }
            set
            {
                dataContainer.Values[StorageKeys.IsOptOut] = value;
            }
        }

        public Session LastSession
        {
            get
            {
                ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)dataContainer.Values[StorageKeys.LastSession];
                if (composite == null)
                    return null;

                return new Session (
                    (long)composite[StorageKeys.LastSessionStartTimestamp], 
                    (string)composite[StorageKeys.LastSessionId], 
                    JsonConvert.DeserializeObject<List<long>>((string)composite[StorageKeys.LastSessionMpids])
                    )
                {
                    BackgroundTime = (long)composite[StorageKeys.LastSessionBackgroundTime],
                    LastEventTimeMillis = (long)composite[StorageKeys.LastSessionLastEnteredBackgroundTime]
                };

            }
            set
            {
                if (value == null)
                {
                    dataContainer.Values.Remove(StorageKeys.LastSession);
                }
                else
                {
                    ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                    composite[StorageKeys.LastSessionId] = value.Id;
                    composite[StorageKeys.LastSessionStartTimestamp] = value.StartTimeMillis;
                    composite[StorageKeys.LastSessionBackgroundTime] = value.BackgroundTime;
                    composite[StorageKeys.LastSessionLastEnteredBackgroundTime] = value.LastEventTimeMillis;
                    composite[StorageKeys.LastSessionMpids] = JsonConvert.SerializeObject(value.Mpids);
                    dataContainer.Values[StorageKeys.LastSession] = composite;
                }
            }
        }

        private ApplicationDataCompositeValue User(long mpid)
        {
            var composite = (ApplicationDataCompositeValue)dataContainer.Values[String.Format(StorageKeys.User, mpid.ToString())];
            return composite ?? new ApplicationDataCompositeValue();
        }

        private void SetUser(long mpid, ApplicationDataCompositeValue user)
        {
            dataContainer.Values[String.Format(StorageKeys.User, mpid.ToString())] = user;
        }

        internal IList<UserIdentity> UserIdentities(long mpid)
        {
            var user = User(mpid);

            if (user.ContainsKey(StorageKeys.UserIdentities))
            {
                var userIdentities = (string)user[StorageKeys.UserIdentities];
                if (!String.IsNullOrEmpty(userIdentities))
                {
                    return JsonConvert.DeserializeObject<List<UserIdentity>>(userIdentities);
                }
            }
            
            return new List<UserIdentity>();
        }

        internal IDictionary<string, object> UserAttributes(long mpid)
        {
            var user = User(mpid);

            if (user.ContainsKey(StorageKeys.UserAttributes))
            {
                var userAttributes = (string)user[StorageKeys.UserAttributes];
                if (!String.IsNullOrEmpty(userAttributes))
                {
                    var attributes = JsonConvert.DeserializeObject<IDictionary<string, object>>(userAttributes);
                    return attributes.ToDictionary(item => item.Key, item =>
                        {
                            if (item.Value is JArray)
                            {
                                return ((JArray)item.Value).ToObject<List<string>>();
                            }
                            else
                            {
                                return item.Value;
                            }
                        }
                    );
                }
            }

            return new Dictionary<string, object>();
        }

        internal void SetUserAttributes(long mpid, IDictionary<string, object> attributes)
        {
            attributes = attributes ?? new Dictionary<string, object>();
            var user = User(mpid);
            var attributesString = JsonConvert.SerializeObject(attributes);
            user[StorageKeys.UserAttributes] = attributesString;
            SetUser(mpid, user);
        }

        internal void SetUserIdentities(long mpid, IEnumerable<UserIdentity> identities)
        {
            identities = identities ?? new List<UserIdentity>();
            var user = User(mpid);
            var identitiesString = JsonConvert.SerializeObject(identities);
            user[StorageKeys.UserIdentities] = identitiesString;
            SetUser(mpid, user);
        }

        public string DeviceApplicationStamp
        {
            get
            {
                return (string) dataContainer.Values[StorageKeys.DeviceApplicationStamp];
            }
            set
            {
                dataContainer.Values[StorageKeys.DeviceApplicationStamp] = value;
            }
        }

        private sealed class StorageKeys
        {
            internal const string ApplicationVersion = "application-version";
            internal const string IsOptOut = "is-opt-out";
            internal const string LastSession = "last-session-container";
            internal const string LastSessionId = "last-session-id";
            internal const string LastSessionStartTimestamp = "last-session-start-timestamp";
            internal const string LastSessionBackgroundTime = "last-session-background-time";
            internal const string LastSessionLastEnteredBackgroundTime = "last-session-last-entered-background";
            internal const string LastSessionMpids = "last-session-mpids";
            internal const string FirstRunTime = "first-run-time";
            internal const string DeviceApplicationStamp = "device-application-stamp";
            internal const string User = "user-{0}";
            internal const string UserId = "user-id";
            internal const string UserIdentities = "user-identities";
            internal const string UserAttributes = "user-attributes";
            internal const string CurrentMpid = "current-user-id";
        }
    }
}