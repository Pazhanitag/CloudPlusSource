using System;
using System.Linq;
using CloudPlus.Database;
using CloudPlus.Entities;
using CloudPlus.Models.ProductConstraint;
using CloudPlus.Resources;

namespace CloudPlus.Services.Database.ProductConstraints
{
    public class ProductConstraintsService: IProductConstraintsService
    {
        private readonly CldpDbContext _dbContext;
        private readonly IObjectSerializer _objectSerializer;

        public ProductConstraintsService(CldpDbContext dbContext, IObjectSerializer objectSerializer)
        {
            _dbContext = dbContext;
            _objectSerializer = objectSerializer;
        }

        public void Insert(ProductConstraintRequestDto productConstraint)
        {
            var product = _dbContext.Products.AsNoTracking()
                .Where(p => p.Id == productConstraint.ProductId &&
                            (productConstraint.Constraint.MandatoryWithProductId != 0 &&
                             p.Id == productConstraint.Constraint.MandatoryWithProductId) &&
                            (productConstraint.Constraint.ConflictingWithProductId != 0 &&
                             p.Id == productConstraint.Constraint.ConflictingWithProductId));

            if (product == null)
            {
                throw new Exception("Invalid product");
            }

            _dbContext.ProductConstraints.Add(new ProductConstraint
            {
                Constraint = _objectSerializer.Serialize(productConstraint.Constraint)
            });

            _dbContext.SaveChanges();
        }
    }
}
