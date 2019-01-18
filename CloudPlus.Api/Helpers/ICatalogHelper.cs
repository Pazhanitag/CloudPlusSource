using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using CloudPlus.Api.ViewModels.Request.Catalog;
using CloudPlus.Api.ViewModels.Response.Catalog;
using CloudPlus.Models.Catalog;

namespace CloudPlus.Api.Helpers
{
    public interface ICatalogHelper
    {
        Task<IEnumerable<CatalogViewModel>> GetCatalogs(int companyId);
        Task<CatalogViewModel> GetCatalog(int companyId, int catalogId);
        Task UpdateFixedRetailPrice(int userId, int companyId, ResellerProductItemModel currentModel, UpdateProductItemViewModel newModel, int catalogId);
        Task UpdateRetailPrice(int userId, int companyId, ResellerProductItemModel currentModel, UpdateProductItemViewModel newModel, int catalogId);
        Task UpdateResellerPrice(int companyId, ResellerProductItemModel currentModel, UpdateProductItemViewModel newModel, int catalogId);
        Task<Tuple<MemoryStream, String>> GetProductDetailsAsMemoryStream(int companyId, int catalogId);
    }
}