
using Newtonsoft.Json;

namespace CloudPlus.Models.ProductConstraint
{
    public class ProductConstraintResponseDto
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Id { get; set; }
        public ProductConstraintContent Expression { get; set; }
    }
}
