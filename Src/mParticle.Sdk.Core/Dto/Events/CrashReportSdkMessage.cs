using System.Collections.Generic;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class CrashReportSdkMessage : SdkMessage
    {
        /// <summary>
        /// Session identifier. Optional.
        /// </summary>
        [JsonProperty("sid")]
        public string SessionId;

        /// <summary>
        /// Session start timestamp in milliseconds since epoch. Optional.
        /// </summary>
        [JsonProperty("sct")]
        public long SessionStartTimestamp;

        /// <summary>
        /// Exception class name. Optional.
        /// </summary>
        [JsonProperty("c")]
        public string Class;

        /// <summary>
        /// Error severity. Required.
        /// </summary>
        [JsonProperty("s")]
        public string Severity;

        /// <summary>
        /// Error message. Required.
        /// </summary>
        [JsonProperty("m")]
        public string Message;

        /// <summary>
        /// Exception stack trace. Optional.
        /// </summary>
        [JsonProperty("st")]
        public string StackTrace;

        /// <summary>
        /// Determines if the exception was handled.
        /// </summary>
        [JsonProperty("eh", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool ExceptionHandled;

        /// <summary>
        /// Topmost context of the exception.  Optional.
        /// </summary>
        [JsonProperty("tc")]
        public string TopmostContext;
        
        /// <summary>
        /// Plausible Labs Crash Report file, as Base-64 string.  Optional.
        /// </summary>
        [JsonProperty("plc")]
        public string PLCrashReportFile;

        [JsonProperty("cs")]
        public CurrentState CurrentState { get; set; }

        /// <summary>
        /// iOS image base address. Optional.
        /// </summary>
        [JsonProperty("iba")]
        public ulong ImageBaseAdress;

        /// <summary>
        /// iOS image size. Optional.
        /// </summary>
        [JsonProperty("is")]
        public ulong ImageSize;

        /// <summary>
        /// Session number that crash occurred on
        /// </summary>
        [JsonProperty("sn")]
        public int SessionNumber;

        /// <summary>
        /// Collection of breadcrumbs leading up to the crash
        /// </summary>
        [JsonProperty("bc")]
        public SdkMessage[] Breadcrumbs;

        /// <summary>
        /// Attributes. Optional.
        /// </summary>
        [JsonProperty("attrs")]
        public IDictionary<string, string> Attributes;
        
        public CrashReportSdkMessage()
            : base(MessageDataType.CrashReportSdkMessage)
        {
        }
    }
}
