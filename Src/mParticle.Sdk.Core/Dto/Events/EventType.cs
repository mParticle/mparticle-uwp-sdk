using System.Runtime.Serialization;

namespace mParticle.Sdk.Core.Dto.Events
{
    public enum EventType : byte
    {
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        [EnumMember(Value = "navigation")]
        Navigation = 1,

        [EnumMember(Value = "location")]
        Location = 2,

        [EnumMember(Value = "search")]
        Search = 3,

        [EnumMember(Value = "transaction")]
        Transaction = 4,

        [EnumMember(Value = "user_content")]
        UserContent = 5,

        [EnumMember(Value = "user_preference")]
        UserPreference = 6,

        [EnumMember(Value = "social")]
        Social = 7,

        [EnumMember(Value = "other")]
        Other = 8,

        [EnumMember(Value = "media")]
        Media = 9,

        [EnumMember(Value = "add_to_cart")]
        ProductAddToCart = 10,

        [EnumMember(Value = "remove_from_cart")]
        ProductRemoveFromCart = 11,

        [EnumMember(Value = "checkout")]
        ProductCheckout = 12,

        [EnumMember(Value = "checkout_option")]
        ProductCheckoutOption = 13,

        [EnumMember(Value = "click")]
        ProductClick = 14,

        [EnumMember(Value = "view_detail")]
        ProductViewDetail = 15,

        [EnumMember(Value = "purchase")]
        ProductPurchase = 16,

        [EnumMember(Value = "refund")]
        ProductRefund = 17,

        [EnumMember(Value = "promotion_view")]
        PromotionView = 18,

        [EnumMember(Value = "promotion_click")]
        PromotionClick = 19,

        [EnumMember(Value = "add_to_wishlist")]
        ProductAddToWishlist = 20,

        [EnumMember(Value = "remove_from_wishlist")]
        ProductRemoveFromWishlist = 21,

        [EnumMember(Value = "impression")]
        ProductImpression = 22,

        [EnumMember(Value = "attribution")]
        Attribution = 23,
    }
}
