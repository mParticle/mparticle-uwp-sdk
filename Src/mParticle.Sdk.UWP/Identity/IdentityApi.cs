using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mParticle.Sdk.Core;
using mParticle.Sdk.Core.Dto.Identity;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace mParticle.Sdk.UWP
{
    /// <summary>
    /// mParticle Identity API.
    /// </summary>
    public sealed class IdentityApi
    {
        /// <summary>
        /// Unknown error: this indicates that the device has no network connectivity. Retry when the device reconnects.
        /// </summary>
        public const int UnknownError = -1;
        /// <summary>
        /// Bad request: inspect the error response and modify as necessary.
        /// </summary>
        public const int BadRequest = 400;
        /// <summary>
        /// Unauthorized: this indicates a bad workspace API key and/or secret.
        /// </summary>
        public const int Unauthorized = 401;
        /// <summary>
        /// Throttle error: this indicates that your mParticle workspace has exceeded its provisioned Identity throughput. Perform an exponential backoff and retry.
        /// </summary>
        public const int ThrottleError = 429;
        /// <summary>
        /// Server error: perform an exponential backoff and retry.
        /// </summary>
        public const int ServerError = 500;
   
        private ILogger logger;
        private readonly PersistenceManager persistenceManager;
        private readonly IdentityApiClient identityApiManager;

        internal IdentityApi(string apiKey, string apiSecret, PersistenceManager persistenceManager)
        {
            this.persistenceManager = persistenceManager;
            this.identityApiManager = new IdentityApiClient(apiKey, apiSecret);
            this.identityApiManager.UserAgent = MParticle.UserAgent;
            this.GenerateDasIfNeeded();
        }

        /// <summary>
        /// Use this to modify the logging interface. Logging is disabled by default.
        /// </summary>
        public ILogger Logger
        {
            get
            {
                return logger;
            }
            set
            {
                this.logger = value;
                this.identityApiManager.Logger = value;
            }
        }

        /// <summary>
        /// Access the currently identified user. 
        /// 
        /// Note: this will be null on the first app-run, prior to the first successful identity API call.
        /// </summary>
        public MParticleUser CurrentUser
        {

            get
            {
                long? mpid = persistenceManager.CurrentMpid;
                if (mpid == null)
                {
                    return null;
                }
                else
                {
                    return new MParticleUser((long)mpid, persistenceManager);
                }
            }

            internal set
            {
                persistenceManager.CurrentMpid = value.Mpid;
            }
        }

        /// <summary>
        /// Subscribe to Identity changes. This will be invoked whenever the current user changes.
        /// </summary>
        public event EventHandler<IdentityStateChangeEventArgs> IdentityStateChange;

        public async Task<IdentityApiResult> IdentifyAsync(IdentityApiRequest identityApiRequest = null)
        {
            var request = BuildIdentityRequest(identityApiRequest);
            var task = identityApiManager.Identify(request);
            return await PerformIdentityCallAsync(identityApiRequest, task);
        }

        public async Task<IdentityApiResult> LogoutAsync(IdentityApiRequest identityApiRequest = null)
        {
            var request = BuildIdentityRequest(identityApiRequest);
            var task = identityApiManager.Logout(request);
            return await PerformIdentityCallAsync(identityApiRequest, task);
        }

        public async Task<IdentityApiResult> LoginAsync(IdentityApiRequest identityApiRequest = null)
        {
            var request = BuildIdentityRequest(identityApiRequest);
            var task = identityApiManager.Login(request);
            return await PerformIdentityCallAsync(identityApiRequest, task);
        }

        internal async Task<IdentityApiResult> ModifyAsync(MParticleUser user, IdentityApiRequest identityApiRequest)
        {
            if (identityApiRequest == null || user == null)
            {
                return new IdentityApiResult()
                {
                    Error = new ErrorResponse()
                    {
                        StatusCode = (int)IdentityApi.BadRequest,
                        Errors = new Error []{ new Error() { Message = "Modify requests must not be empty." } }

                    }
                };
            }
            ModifyRequest request = BuildModifyRequest(user, identityApiRequest);
            var task = identityApiManager.Modify(user.Mpid, request);
            return await PerformIdentityCallAsync(identityApiRequest, task, user);
        }

        private async Task<IdentityApiResult> PerformIdentityCallAsync(IdentityApiRequest originalRequest, Task<object> identityTask, MParticleUser user = null)
        {
            try
            {
                var response = await identityTask;
                if (response is IdentityResponse)
                {
                    var newUser = user ?? new MParticleUser(long.Parse(((IdentityResponse)response).Mpid), persistenceManager);
                    newUser.UserIdentities = originalRequest.UserIdentities;
                    if (CurrentUser == null || CurrentUser.Mpid != newUser.Mpid)
                    {
                        CurrentUser = newUser;
                        try
                        {
                            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                () =>
                                {
                                    originalRequest.UserAliasDelegate?.Invoke(CurrentUser, newUser);
                                    this.IdentityStateChange?.Invoke(this, new IdentityStateChangeEventArgs(originalRequest, newUser));
                                });

                        }
                        catch (Exception ex)
                        {
                            this.Logger?.Log(new LogEntry(LoggingEventType.Error, "Error while invoking IdentityStateChange listener" + ex.ToString()));
                        }
                    }

                    return new IdentityApiResult()
                    {
                        User = newUser
                    };
                }
                else
                {
                    return new IdentityApiResult() { Error = (ErrorResponse)response };
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Log(new LogEntry(LoggingEventType.Error, "Error while performing identity request: " + ex.ToString()));

                return new IdentityApiResult()
                {
                    Error = new ErrorResponse()
                    {
                        StatusCode = (int)IdentityApi.UnknownError,
                        Errors = new Error[] { new Error() { Message = ex.Message } }
                    }
                };
            }
        }

        internal static void AddUserIdentities(Identities identities, IdentityApiRequest identityApiRequest)
        {
            if (identityApiRequest != null)
            {
                var userIdentities = identityApiRequest.UserIdentities;
                foreach (var identity in userIdentities)
                {
                    identities[(IdentityType)identity.Key] = identity.Value;
                }
            }
        }

        internal void AddDeviceIdentities(Identities identities)
        {
            identities[IdentityType.DeviceApplicationStamp] = GenerateDasIfNeeded();
            identities[IdentityType.MicrosoftAdvertisingId] = DeviceInfoBuilder.QueryAdvertisingId();
            identities[IdentityType.MicrosoftPublisherId] = DeviceInfoBuilder.QueryPublisherId();
        }

        internal string GenerateDasIfNeeded()
        {
            var das = this.persistenceManager.DeviceApplicationStamp;
            if (string.IsNullOrEmpty(das))
            {
                das = Guid.NewGuid().ToString();
                this.persistenceManager.DeviceApplicationStamp = das;
            }
            return das;
        }

        private void PopulateBaseIdentityRequest(IdentityBaseRequest identityRequest)
        {
            identityRequest.ClientSdk = new ClientSdk()
            {
                Platform = Platform.Xbox,
                SdkVendor = "mParticle",
                SdkVersion = MParticle.SdkVersion
            };
            identityRequest.SourceRequestId = Guid.NewGuid().ToString();
            identityRequest.RequestTimestampMs = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            identityRequest.Environment =
                MParticle.Instance.Options.DevelopmentMode ? Core.Dto.Identity.Environment.Development : Core.Dto.Identity.Environment.Production;

        }

        private ModifyRequest BuildModifyRequest(MParticleUser user, IdentityApiRequest identityApiRequest)
        {
            var modifyRequest = new ModifyRequest() { IdentityChanges = new List<IdentityChange>() };
            PopulateBaseIdentityRequest(modifyRequest);
            var updatedIdentities = identityApiRequest.UserIdentities;
            var currentIdentities = user.UserIdentities;
            foreach (var identity in updatedIdentities)
            {
                var change = new IdentityChange()
                {
                    NewValue = identity.Value,
                    IdentityType = (IdentityType)identity.Key
                };
                if (currentIdentities.ContainsKey(identity.Key))
                {
                    change.OldValue = currentIdentities[identity.Key];
                }
                (modifyRequest.IdentityChanges as List<IdentityChange>).Add(change);

            }
            return modifyRequest;
        }

        private IdentityRequest BuildIdentityRequest(IdentityApiRequest identityApiRequest)
        {
            var identityRequest = new IdentityRequest();
            PopulateBaseIdentityRequest(identityRequest);
            var identities = new Identities();
            AddDeviceIdentities(identities);
            AddUserIdentities(identities, identityApiRequest);
            identityRequest.KnownIdentities = identities;
            return identityRequest;
        }

     
    }

    /// <summary>
    /// Provides the resulting user from an Identity API request.
    /// </summary>
    public sealed class IdentityStateChangeEventArgs : EventArgs
    {
        internal IdentityStateChangeEventArgs(IdentityApiRequest request, MParticleUser user)
        {
            Request = request;
            User = user;
        }

        /// <summary>
        /// Gets the request that triggered the Identity-state change
        /// </summary>
        public IdentityApiRequest Request { get; }

        /// <summary>
        /// Gets the newly identified user.
        /// </summary>
        public MParticleUser User { get; }
    }
}