using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Models.Catalog;

namespace CloudPlus.Services.Database.Catalog
{
    public interface ICustomerCatalogService
    {
        Task<IEnumerable<CustomerProductModel>> GetCustomerProducts(int companyId);
        Task<CustomerProductModel> GetCustomerProduct(int companyId, int productId);
    }
}