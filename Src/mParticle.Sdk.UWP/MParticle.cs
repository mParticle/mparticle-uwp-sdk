using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mParticle.Sdk.Core;
using mParticle.Sdk.Core.Dto.Events;
using mParticle.Sdk.Core.Dto.Identity;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace mParticle.Sdk.UWP
{
    /// <summary>
    /// mParticle UWP SDK API. Use this singleton to access all mParticle event and identity APIs.
    /// </summary>
    public class MParticle
    {
        public const string SdkVersion = "1.0.2";
        private static volatile MParticle instance;
        private static readonly object mutex = new object();
        private readonly SessionManager sessionManager;
        private ILogger logger;
        private PersistenceManager persistenceManager;
        private EventsApiClient apiManager;
        private Application application;
        private MessageManager messageManager;

        private MParticle(Application application, MParticleOptions options)
        {
            this.Options = options ?? throw new InvalidOperationException("MParticle must be initialized with a non-null MParticleOptions object.");
            this.application = application;
            this.application.Suspending += Application_Suspending;
            this.application.Resuming += Application_Resuming;
            this.application.EnteredBackground += Application_EnteredBackground;
            this.application.LeavingBackground += Application_LeavingBackground;

            this.apiManager = new EventsApiClient(options.ApiKey, options.ApiSecret);
            this.apiManager.UploadInterval = TimeSpan.FromSeconds(options.UploadIntervalSeconds);
            this.apiManager.UserAgent = UserAgent;
            this.apiManager.Logger = options.Logger;
            this.persistenceManager = new PersistenceManager(options);
            this.persistenceManager.Initialize(Package.Current.Id.Version);
            this.messageManager = new MessageManager(apiManager, persistenceManager);
            this.messageManager.Enabled = !persistenceManager.IsOptOut;
            this.sessionManager = new SessionManager(messageManager, persistenceManager);
            this.Identity = new IdentityApi(options.ApiKey, options.ApiSecret, persistenceManager);
            this.Identity.Logger = options.Logger;
        }

        /// <summary>
        /// Initializes the SDK. This method must be called prior to any other method. 
        /// </summary>
        /// <example> 
        /// This method should be called within 
        /// the <see cref="Application.OnLaunched(Windows.ApplicationModel.Activation.LaunchActivatedEventArgs)"/> method.
        /// <code>
        /// 
        /// protected override void OnLaunched(LaunchActivatedEventArgs launchArgs)
        /// {
        ///    MParticleOptions options = new MParticleOptions()
        ///    {
        ///        ApiKey = "REPLACE ME",
        ///        ApiSecret = "REPLACE ME",
        ///        LaunchArgs = launchArgs
        ///    };
        ///
        ///    MParticle.Start(options);
        /// }
        /// </code>
        /// </example>
        /// <param name="options">Required - at minimum you must supply an mParticle workspace key and secret.</param>
        public static async Task<IdentityApiResult> StartAsync(MParticleOptions options)
        {
            if (instance == null)
            {
                lock (mutex)
                {
                    if (instance == null)
                    {
                        instance = new MParticle(Application.Current, options);
                        instance.Application_Launched();
                        instance.Identity.IdentityStateChange += instance.Identity_IdentityStateChange;
                    }
                }
                var request = options.IdentifyRequest ?? IdentityApiRequest.WithUser(instance.Identity.CurrentUser).Build();
                return await instance.Identity.IdentifyAsync(request);
            }
            return new IdentityApiResult()
            {
                Error = new ErrorResponse()
                {
                    StatusCode = IdentityApi.UnknownError,
                    Errors = new Error[] {
                        new Error()
                        {
                            Message = "The SDK has already started."
                        }
                    }
                }
            };
        }

        /// <summary>
        /// mParticle singleton instance. Apps must call MParticle.Start prior to accessing this instance.
        /// 
        /// This is also settable to help in unit testing.
        /// </summary>
        ///<exception cref="System.InvalidOperationException">Thrown when accessed prior to MParticle.Start()</exception>
        public static MParticle Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (mutex)
                    {
                        if (instance == null)
                            throw new InvalidOperationException("You must call MParticle.Start() prior to MParticle.Instance.");
                    }
                }

                return instance;
            }
            set
            {
                instance = value;
            }
        }

        internal const string UserAgent = "mParticle UWP Sdk/" + SdkVersion;

        /// <summary>
        /// Access the Identity API
        /// </summary>
        public IdentityApi Identity { get; }

        /// <summary>
        /// Access the options used the start the SDK
        /// </summary>
        internal MParticleOptions Options { get; }

        /// <summary>
        /// Gets or sets an ILogger instance.
        /// </summary>
        public ILogger Logger
        {
            get
            {
                return this.logger;
            }
            set {
                this.logger = value;
                this.apiManager.Logger = value;
                this.Identity.Logger = value;
            }
        }

        /// <summary>
        /// Log a CustomEvent to be uploaded.
        /// </summary>
        /// <param name="customEvent"></param>
        public void LogEvent(CustomEvent customEvent)
        {
            var message = new EventSdkMessage()
            {
                SessionId = sessionManager.CurrentSession?.Id,
                EventName = customEvent.EventName,
                EventType = (EventType)customEvent.EventType,
                EventLength = customEvent.EventLength,
                Attributes = customEvent.CustomAttributes?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                CustomFlags = customEvent.CustomFlags?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            };
            messageManager.LogMessage(message);
        }

        /// <summary>
        /// Log a Screen-view to be uploaded
        /// 
        /// </summary>
        /// <param name="screenName"></param>
        /// <param name="customAttributes"></param>
        /// <param name="customFlags"></param>
        public void LogScreen(string screenName, IDictionary<String, String> customAttributes = null,
            IDictionary<String, String> customFlags = null)
        {
            var message = new ScreenViewSdkMessage()
            {
                SessionId = sessionManager.CurrentSession?.Id,
                ScreenName = screenName,
                Attributes = customAttributes,
                CustomFlags = customFlags

            };
            messageManager.LogMessage(message);
        }

        /// <summary>
        /// Disable or enable the mParticle SDK to opt the user out of all tracking. Calling
        /// this method will disable all uploads.
        /// </summary>
        /// <param name="optOut"></param>
        public void SetOptOut(bool optOut)
        {
            var message = new OptOutSdkMessage()
            {
                SessionId = sessionManager.CurrentSession?.Id,
                SessionStartTimestamp = (long)sessionManager.CurrentSession?.StartTimeMillis,
                OptOut = optOut,

            };
            messageManager.LogMessage(message);
            messageManager.Enabled = !optOut;
            persistenceManager.IsOptOut = optOut;
        }

        private void Identity_IdentityStateChange(object sender, IdentityStateChangeEventArgs e)
        {
            sessionManager.CurrentSession?.AddMpid(e.User);
            persistenceManager.LastSession = sessionManager.CurrentSession;
            messageManager.LogPending(e.User);
        }

        void Application_Resuming(object sender, object e)
        {
            this.apiManager.Resume();
        }

        async void Application_Suspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            try
            {
                await this.apiManager.SuspendAsync();
            }
            finally
            {
                deferral.Complete();
            }
        }

        internal void Application_Launched()
        {
            this.sessionManager.Application_Launched();
        }

        private void Application_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            this.sessionManager.Application_LeavingBackground();
        }

        private void Application_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            this.sessionManager.Application_EnteredBackground();
        }
    }
}