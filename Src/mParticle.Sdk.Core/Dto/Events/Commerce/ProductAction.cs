using System.Collections.Generic;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events.Commerce
{
    public sealed class ProductAction
    {
        [JsonProperty("an")]
        public string ActionType; // enum: add_to_cart, remove_from_cart, checkout, click, view, view_detail, purchase, refund, add_to_wishlist, remove_from_wishlist

        [JsonProperty("cs")]
        public int CheckoutStep;

        [JsonProperty("co")]
        public string CheckoutOptions;

        [JsonProperty("pal")]
        public string ProductActionList;

        [JsonProperty("pls")]
        public string ProductListSource;

        [JsonProperty("ti")]
        public string TransactionId;

        [JsonProperty("ta")]
        public string Affiliation;

        [JsonProperty("tr")]
        public decimal TotalAmount;

        [JsonProperty("tt")]
        public decimal TaxAmount;

        [JsonProperty("ts")]
        public decimal ShippingAmount;

        [JsonProperty("tcc")]
        public string CouponCode;

        [JsonProperty("pl")]
        public IList<Product> Products;
    }
}
