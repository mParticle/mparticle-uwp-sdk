using System.Collections.Generic;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events.Commerce
{
    public sealed class Product
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("nm")]
        public string Name;

        [JsonProperty("br")]
        public string Brand;

        [JsonProperty("ca")]
        public string Category;

        [JsonProperty("va")]
        public string Variant;

        [JsonProperty("ps")]
        public int Position;

        [JsonProperty("pr")]
        public decimal Price;

        [JsonProperty("qt")]
        public decimal Quantity;

        [JsonProperty("cc")]
        public string CouponCode;

        [JsonProperty("act")]
        public long AddedToCartTimestamp;

        [JsonProperty("tpa")]
        public decimal TotalProductAmount;

        [JsonProperty("attrs")]
        public IDictionary<string, string> Attributes;
    }
}
