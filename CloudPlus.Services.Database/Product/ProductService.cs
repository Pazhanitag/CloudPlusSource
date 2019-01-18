using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database;

namespace CloudPlus.Services.Database.Product
{
    public class ProductService : IProductService
    {
        private readonly CldpDbContext _dbContext;

        public ProductService(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductModel> GetProduct(int productId)
        {
            var product = await _dbContext.Products
                    .Include(p => p.ProductItems)
                    .FirstOrDefaultAsync(p => p.Id == productId);
            
            if(product == null)
                throw new Exception($"Could not find product with id {productId}");

            return new ProductModel
            {
                Id = product.Id,
                ImgUrl = product.ImgUrl,
                Name = product.Name,
                Description = product.Description,
                Ord = product.Ord,
                Vendor = product.Vendor,
                ProductItems = product.ProductItems.Select(pi => new ProductItemModel
                {
                    Name = pi.Name,
                    Description = pi.Description,
                    BillingType = pi.BillingType,
                    BillingCycle = pi.BillingCycle,
                    KnowledgebaseLink = pi.KnowledgebaseLink,
                    VideoLink = pi.VideoLink,
                    Status = pi.Status,
                    DefaultMarkupPercentage = pi.DefaultMarkupPercentage,
                    Identifier = pi.Identifier,
                    IntegrationType = pi.IntegrationType,
                    IsAddon = pi.IsAddon,
                    ProductSuppressible = pi.ProductSuppressible
                })
            };
        }
    }
    
}