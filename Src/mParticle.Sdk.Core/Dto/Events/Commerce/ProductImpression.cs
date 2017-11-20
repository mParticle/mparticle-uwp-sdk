using System.Collections.Generic;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events.Commerce
{
    public sealed class ProductImpression
    {
        [JsonProperty("pil")]
        public string ProductImpressionList;

        [JsonProperty("pl")]
        public IList<Product> Products;
    }
}
