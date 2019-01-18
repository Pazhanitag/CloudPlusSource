using System.Threading.Tasks;
using CloudPlus.Models.Catalog;


namespace CloudPlus.Services.Database.Catalog
{
    public interface ICatalogProductItemService
    {
        Task UpdateFixedRetailPrice(int companyId, int productItemId, int catalogId, bool fixedRetailPrice);
        Task ChangeResellerPrice(int companyId, int productItemId, int catalogId, decimal newResellerPrice);
        Task AddProductItemToCatalog(CatalogProductModel productCatalogModel);
        Task RemoveProductItemFromCatalog(int companyId, int productItemId, int catalogId);
        Task ChangeRetailPrice(int companyId, int productItemId, int catalogId, decimal newRetailPrice);
        Task ChangeProductAvailability(int companyId, int catalogId, int productItemId, bool available);
    }
}