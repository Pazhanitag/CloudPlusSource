using System.Collections.Generic;
using CloudPlus.Models.Category;
using CloudPlus.Models.Enums.Product;
using CloudPlus.Models.ProductConstraint;
using Newtonsoft.Json;

namespace CloudPlus.Models.Product
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int BillingType { get; set; }
        public int BillingCycle { get; set; }
        public bool ProductSuppressible { get; set; }
        public string KnowledgebaseLink { get; set; }
        public string VideoLink { get; set; }
        public int IntegrationType { get; set; }
        public bool IsAddon { get; set; }
        public CategoryModel Category { get; set; }
        public CategoryModel SubCategory { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ProductConstraintResponseDto Constraint { get; set; }
	    public decimal Price { get; set; }

		// additional fields moved from the former ProductInsertModel
	    public int CategoryId { get; set; }
	    public List<ProductConstraintRequestDto> ProductConstraints { get; set; }
	}
}
