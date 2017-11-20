using System.Collections.Generic;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events.Commerce
{
    public sealed class ProductBag
    {
        [JsonProperty("pl")]
        public IList<Product> Products;
    }
}
