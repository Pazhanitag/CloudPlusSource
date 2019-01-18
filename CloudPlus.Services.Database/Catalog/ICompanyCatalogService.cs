using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using CloudPlus.Models.Catalog;

namespace CloudPlus.Services.Database.Catalog
{
    public interface ICompanyCatalogService
    {
        Task<IEnumerable<CatalogCompanyModel>> GetCompaniesAssignedToCatalog(int companyId, int catalogId);
        Task<CatalogModel> GetCompanyAssignedCatalog(int companyId);
        Task AssignDefaultCatalogToCompany(int parentCompanyId, int companyId);
        Task AssignCatalogToCompany(int parentCompanyId, int companyId, int catalogId);
        Task<IEnumerable<CatalogModel>> GetCompanyCatalogs(int companyId);
        Task DeleteCatalog(int companyId, int catalogId);
        Task CreateNewCatalog(int companyId, int sourceCatalogId, CatalogModel resellerCatalogModel);
        Task UpdateCompanyCatalog(int companyId, int catalogId, CatalogModel resellerCatalogModel);
        Task<CatalogModel> GetCompanyCatalog(int companyId, int catalogId);
        Task RemoveCompanyFromAssignedCatalog(int parentCompanyId, int companyId);
        Task ChangeDefaultCompanyCatalog(int companyId, int newDefaultCatalogId);
        Task GenerateDefaultCompanyCatalog(int companyId);
        Task<IEnumerable<CatalogCompanyModel>> GetMyCompaniesAssignedCatalogs(int companyId);
        Task<DataTable> SendProductDetailsAsEmail(int companyId,int catalogId);
        Task<Tuple<DataTable,String>> GetProductDownloadData(int companyId, int catalogId);

    }
}