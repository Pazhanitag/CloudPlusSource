using System.Collections.Generic;
using System.Linq;
using CloudPlus.Database;
using CloudPlus.Services.Database.Product;

namespace CloudPlus.Services.Database.ProductItem
{
    public class ProductItemService : IProductItemService
    {
        private readonly CldpDbContext _dbContext;

        public ProductItemService(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ProductItemModel> GetProductItems()
        {
            var productItems = _dbContext.ProductItems.Select(item => new ProductItemModel
            {
                Id = item.Id,
                Identifier = item.Identifier,
                Name = item.Name,
                Description = item.Description,
                Status = item.Status,
                BillingType = item.BillingType,
                BillingCycle = item.BillingCycle,
                IsAddon = item.IsAddon,
                ProductSuppressible = item.ProductSuppressible,
                KnowledgebaseLink = item.KnowledgebaseLink,
                VideoLink = item.VideoLink,
                DefaultMarkupPercentage = item.DefaultMarkupPercentage,
                IntegrationType = item.IntegrationType,
                CreateDate = item.CreateDate,
                UpdateDate = item.UpdateDate
            });

            return productItems;
        }
    }
}
