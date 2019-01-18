
using CloudPlus.Models.Product;

namespace CloudPlus.Models.ProductConstraint
{
    public class ProductConstraintRequestDto
    {
        public int ProductId { get; set; }
        public ProductConstraintContent Constraint { get; set; }
    }
}
