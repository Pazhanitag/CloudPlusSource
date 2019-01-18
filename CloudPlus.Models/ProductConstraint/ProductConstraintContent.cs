
using Newtonsoft.Json;

namespace CloudPlus.Models.ProductConstraint
{
    public class ProductConstraintContent
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ConflictingWithProductId { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MandatoryWithProductId { get; set; }
    }
}
