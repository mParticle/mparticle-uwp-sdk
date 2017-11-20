using System.Collections.Generic;
using mParticle.Sdk.Core.Dto.Events.Commerce;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events
{
    public sealed class CommerceSdkMessage : SdkMessage
    {
        /// <summary>
        /// Session identifier. Required.
        /// </summary>
        [JsonProperty("sid")]
        public string SessionId;

        /// <summary>
        /// Session start timestamp. Required.
        /// </summary>
        [JsonProperty("sct")]
        public long SessionStartTimestamp;

        /// <summary>
        /// Event timestamp. Required.
        /// </summary>
        [JsonProperty("est")]
        public long EventStartTimestamp;

        /// <summary>
        /// Event length. Optional.
        /// </summary>
        [JsonProperty("el")]
        public long EventLength;

        /// <summary>
        /// Attributes. Optional.
        /// </summary>
        [JsonProperty("attrs")]
        public IDictionary<string, string> Attributes;

        /// <summary>
        /// Location. Optional.
        /// </summary>
        [JsonProperty("lc")]
        public Location Location;

        [JsonProperty("cs")]
        public CurrentState CurrentState { get; set; }

        [JsonProperty("en")]
        public int EventNum { get; set; }

        [JsonProperty("pd")]
        public ProductAction ProductAction;

        [JsonProperty("pm")]
        public PromotionAction PromotionAction;

        [JsonProperty("pi")]
        public IList<ProductImpression> ProductImpressions;

        [JsonProperty("sc")]
        public ShoppingCart ShoppingCart;

        [JsonProperty("cu")]
        public string CurrencyCode;

        /// <summary>
        /// Screen name where the commerce event occurs
        /// </summary>
        [JsonProperty("sn")]
        public string ScreenName;

        /// <summary>
        /// Indicates this is a non-interactive event
        /// </summary>
        [JsonProperty("ni")]
        public bool IsNonInteractive;

        public CommerceSdkMessage()
            : base(MessageDataType.CommerceSdkMessage)
        {
        }
    }

}
