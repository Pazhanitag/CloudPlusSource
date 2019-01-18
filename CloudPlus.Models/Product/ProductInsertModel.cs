using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CloudPlus.Models.Enums.Product;
using CloudPlus.Models.ProductConstraint;

namespace CloudPlus.Models.Product
{
    public class ProductInsertModelw
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public BillingType BillingType { get; set; }
        [Required]
        public BillingCycle BillingCycle { get; set; }
        public bool IsAddon { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public List<ProductConstraintRequestDto> ProductConstraints { get; set; }
    }
}
