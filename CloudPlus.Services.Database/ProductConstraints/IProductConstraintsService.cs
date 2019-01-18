using CloudPlus.Models.ProductConstraint;

namespace CloudPlus.Services.Database.ProductConstraints
{
    public interface IProductConstraintsService
    {
        void Insert(ProductConstraintRequestDto productConstraint);
    }
}
