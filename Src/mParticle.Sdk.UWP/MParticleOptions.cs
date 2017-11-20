using System;
using mParticle.Sdk.Core;
using Windows.ApplicationModel.Activation;

namespace mParticle.Sdk.UWP
{
    /// <summary>
    /// mParticle options - an instance of this class is required to initialize the SDK via <see cref="MParticle.StartAsync(MParticleOptions)"/>
    /// </summary>
    public sealed class MParticleOptions
    {
        public const string DefaultDataContainer = "mParticleSdk";
        public const int DefaultSessionTimeoutSeconds = 60;
        public const int DefaultUploadIntervalSeconds = 10;

        public string ApiKey { get; }
        public string ApiSecret { get; }
        public bool DevelopmentMode { get; }
        public int UploadIntervalSeconds { get; }
        public long SessionTimeoutSeconds { get; }
        public string DataContainer { get; }
        public ILogger Logger { get; }
        public IdentityApiRequest IdentifyRequest { get; }
        public LaunchActivatedEventArgs LaunchArgs { get; }

        internal MParticleOptions(MParticleOptionsBuilder builder)
        {
            if (string.IsNullOrEmpty(builder.apiKey) || string.IsNullOrEmpty(builder.apiSecret))
            {
                throw new ArgumentNullException("You must supply an mParticle workspace key and secret.");
            }
            this.ApiKey = builder.apiKey;
            this.ApiSecret = builder.apiSecret;
            this.DevelopmentMode = builder.developmentMode;
            this.UploadIntervalSeconds = builder.uploadIntervalSeconds ?? DefaultUploadIntervalSeconds;
            this.SessionTimeoutSeconds = builder.sessionTimeoutSeconds ?? DefaultSessionTimeoutSeconds;
            this.DataContainer = builder.dataContainer ?? DefaultDataContainer;
            this.Logger = builder.logger;
            this.IdentifyRequest = builder.identifyRequest;
            this.LaunchArgs = builder.launchArgs;
        }

        /// <summary>
        /// Convenient method to create an MParticleOptionsBuilder
        /// </summary>
        /// <param name="apiKey">mParticle workspace key (required)</param>
        /// <param name="apiSecret">mParticle workspace secret (required)</param>
        /// <param name="developmentMode">Initialize the SDK in production or development mode. Defaults to production.</param>
        /// <returns></returns>
        public static MParticleOptionsBuilder Builder(string apiKey, string apiSecret, bool developmentMode = false)
        {
            return new MParticleOptionsBuilder(apiKey, apiSecret, developmentMode);
        }

        /// <summary>
        /// PParticleOptionsBuilder - use this class to create immutable MParticleOptions objects.
        /// </summary>
        public sealed class MParticleOptionsBuilder
        {
            internal readonly string apiKey;
            internal readonly string apiSecret;
            internal readonly bool developmentMode;
            internal int? uploadIntervalSeconds;
            internal int? sessionTimeoutSeconds;
            internal string dataContainer;
            internal ILogger logger;
            internal IdentityApiRequest identifyRequest;
            internal LaunchActivatedEventArgs launchArgs;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="apiKey">mParticle workspace key (required)</param>
            /// <param name="apiSecret">mParticle workspace secret (required)</param>
            /// <param name="developmentMode">Initialize the SDK in production or development mode. Defaults to production.</param>
            public MParticleOptionsBuilder(string apiKey, string apiSecret, bool developmentMode = false)
            {
                this.apiKey = apiKey;
                this.apiSecret = apiSecret;
                this.developmentMode = developmentMode;
            }

            /// <summary>
            /// Set the underlying upload interval. 
            /// 
            /// Defaults to <see cref="DefaultUploadIntervalSeconds"/>.
            /// </summary>
            /// <param name="interval"></param>
            /// <returns></returns>
            public MParticleOptionsBuilder UploadIntervalSeconds(int interval)
            {
                this.uploadIntervalSeconds = interval;
                return this;
            }

            /// <summary>
            /// Set the session timeout. Sessions will timeout after the application has suspended for this period of time.
            /// 
            /// Defaults to <see cref="DefaultSessionTimeoutSeconds"/>.
            /// </summary>
            /// <param name="timeout"></param>
            /// <returns></returns>
            public MParticleOptionsBuilder SessionTimeoutSeconds(int timeout)
            {
                this.sessionTimeoutSeconds = timeout;
                return this;
            }

            /// <summary>
            /// Set the data container name to use for persistent storage.
            /// 
            /// Defaults to <see cref="DefaultDataContainer"/>.
            /// </summary>
            /// <param name="dataContainer"></param>
            /// <returns></returns>
            public MParticleOptionsBuilder DataContainer(string dataContainer)
            {
                this.dataContainer = dataContainer;
                return this;
            }

            /// <summary>
            /// Set a logging interface. Logging is disabled by default.
            /// </summary>
            /// <param name="logger"></param>
            /// <returns></returns>
            public MParticleOptionsBuilder Logger(ILogger logger)
            {
                this.logger = logger;
                return this;
            }

            /// <summary>
            /// Set the initial Identify request to perform.
            /// 
            /// By default, the SDK will perform an Identify request for the last user of the device.
            /// </summary>
            /// <param name="identityApiRequest"></param>
            /// <returns></returns>
            public MParticleOptionsBuilder IdentifyRequest(IdentityApiRequest identityApiRequest)
            {
                this.identifyRequest = identityApiRequest;
                return this;
            }

            /// <summary>
            /// Supply the launch args of the application to collect deep-linking analytics.
            /// </summary>
            /// <param name="launchArgs"></param>
            /// <returns></returns>
            public MParticleOptionsBuilder LaunchArgs(LaunchActivatedEventArgs launchArgs)
            {
                this.launchArgs = launchArgs;
                return this;
            }

            /// <summary>
            /// Build the object. Pass the result of this into <see cref="MParticle.StartAsync(MParticleOptions)"/>.
            /// </summary>
            /// <returns>An immutable MParticleOptions object</returns>
             ///<exception cref="System.ArgumentNullException">Thrown when no API key and secret are provided.</exception>
            public MParticleOptions Build()
            {
                return new MParticleOptions(this);
            }
        }
    }
}
