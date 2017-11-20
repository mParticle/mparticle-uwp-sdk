using System.Collections.Generic;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Events.Commerce
{
    public sealed class PromotionAction
    {
        [JsonProperty("an")]
        public string ActionType; // enum: view, click

        [JsonProperty("pl")]
        public IList<Promotion> Promotions;
    }
}
