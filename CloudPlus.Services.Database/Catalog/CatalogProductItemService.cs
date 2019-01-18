using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Entities.Catalog;
using CloudPlus.Exceptions.Catalog;
using CloudPlus.Exceptions.Company;
using CloudPlus.Models.Catalog;

namespace CloudPlus.Services.Database.Catalog
{
    public class CatalogProductItemService : ICatalogProductItemService
    {
        private readonly CldpDbContext _dbContext;
        private readonly ICatalogUtilities _catalogUtilities;
        public CatalogProductItemService(CldpDbContext dbContext, ICatalogUtilities catalogUtilities)
        {
            _dbContext = dbContext;
            _catalogUtilities = catalogUtilities;
        }
        public async Task UpdateFixedRetailPrice(int companyId, int productItemId, int catalogId, bool fixedRetailPrice)
        {
            var company = await _dbContext.Companies
                .Include(c => c.Parent)
                .Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems))
                .Include(c => c.MyCompanies.Select(mc => mc.CompanyCatalogs)).FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var selectedCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogId == catalogId);

            if (selectedCatalog == null)
                throw new Exception($"Catalog with catalogId {catalogId} does not exist in CompanyCatalogs of company {companyId}");

            var catalogProductItems = selectedCatalog.Catalog.CatalogProductItems.Where(p => p.ProductItemId == productItemId).ToList();

            catalogProductItems.ForEach(c =>
            {
                c.FixedRetailPrice = fixedRetailPrice;

                if (company.Parent == null || !fixedRetailPrice) return;

                var assignedCatalog = company.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.Assigned);

                if (assignedCatalog == null)
                    throw new NoAssignedCatalogException($"CompanyId: {companyId}");

                var assignedCatalogProduct = assignedCatalog.Catalog.CatalogProductItems.FirstOrDefault(p => p.ProductItemId == c.ProductItemId);

                if (assignedCatalogProduct == null)
                    throw new NoProductInProductCatalogException($"CompanyId: {companyId}, CatalogId: {assignedCatalog.Catalog.Id}, ProductId: {c.ProductItemId}");

                c.RetailPrice = assignedCatalogProduct.RetailPrice;
            });

            foreach (var myCompany in company.MyCompanies.Where(c => c.CompanyCatalogs.Any(cc => cc.CatalogType == CatalogType.Assigned && cc.CatalogId == catalogId)))
            {
                foreach (var myCompanyCompanyCatalog in myCompany.CompanyCatalogs)
                {
                    await UpdateFixedRetailPrice(myCompany.Id, productItemId, myCompanyCompanyCatalog.CatalogId,
                        fixedRetailPrice);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task ChangeResellerPrice(int companyId, int productItemId, int catalogId, decimal newResellerPrice)
        {
            var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var companyCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogId == catalogId);

            if (companyCatalog == null)
                throw new NonExistingCompanyCatalogException($"CompanyId: {company.Id}, CatalogI: {catalogId}");

            var catalogProductItem = companyCatalog.Catalog.CatalogProductItems.FirstOrDefault(cp => cp.ProductItemId == productItemId);

            if (catalogProductItem == null)
                throw new NullReferenceException($"No product with id {productItemId} in catalog {catalogId}");

            catalogProductItem.ResellerPrice = newResellerPrice;

            await _dbContext.SaveChangesAsync();
        }
        public async Task AddProductItemToCatalog(CatalogProductModel productCatalogModel)
        {
            var company = await _dbContext.Companies.Include(c => c.Parent).Include(c => c.MyCompanies.Select(mc => mc.CompanyCatalogs))
                .Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems)).FirstOrDefaultAsync(c => c.Id == productCatalogModel.CompanyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {productCatalogModel.CompanyId}");

            var assignedCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogType == CatalogType.Assigned);

            if (assignedCatalog == null)
                throw new NoAssignedCatalogException($"CompanyId: {productCatalogModel.CompanyId}");

            var assignedCatalogProductItem = assignedCatalog.Catalog.CatalogProductItems.FirstOrDefault(p => p.ProductItemId == productCatalogModel.ProductItemId);

            if (assignedCatalogProductItem == null)
                throw new NoProductInProductCatalogException($"CompanyId: {productCatalogModel.CompanyId}, CatalogId: {assignedCatalog.Catalog.Id}, ProductId: {productCatalogModel.ProductItemId}");

            var companyCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogId == productCatalogModel.CatalogId && c.CatalogType == CatalogType.MyCatalog);

            if (companyCatalog == null)
                throw new NonExistingCompanyCatalogException($"CompanyId: {company.Id}, CatalogI: {productCatalogModel.CatalogId}");

            if (companyCatalog.Catalog.CatalogProductItems.All(p => p.ProductItemId != productCatalogModel.ProductItemId))
            {
                companyCatalog.Catalog.CatalogProductItems.Add(new CatalogProductItem
                {
                    CatalogId = productCatalogModel.CatalogId,
                    ProductItemId = productCatalogModel.ProductItemId,
                    ResellerPrice = productCatalogModel.ResellerPrice,
                    RetailPrice = company.Parent == null ? productCatalogModel.RetailPrice : assignedCatalogProductItem.FixedRetailPrice ? assignedCatalogProductItem.RetailPrice : productCatalogModel.RetailPrice,
                    FixedRetailPrice = company.Parent == null ? productCatalogModel.FixedRetailPrice : assignedCatalogProductItem.FixedRetailPrice,
                    Available = true
                });
            }

            foreach (var myCompany in company.MyCompanies.Where(c => c.CompanyCatalogs.Any(cc => cc.CatalogType == CatalogType.Assigned && cc.CatalogId == productCatalogModel.CatalogId)))
            {
                foreach (var myCompanyCatalog in myCompany.CompanyCatalogs.Where(c => c.CatalogType == CatalogType.MyCatalog))
                {
                    await AddProductItemToCatalog(new CatalogProductModel
                    {
                        CompanyId = myCompany.Id,
                        ProductItemId = productCatalogModel.ProductItemId,
                        CatalogId = myCompanyCatalog.CatalogId,
                        FixedRetailPrice = company.Parent == null ? productCatalogModel.FixedRetailPrice : assignedCatalogProductItem.FixedRetailPrice,
                        RetailPrice = company.Parent == null ? productCatalogModel.RetailPrice : assignedCatalogProductItem.FixedRetailPrice ? assignedCatalogProductItem.RetailPrice : productCatalogModel.RetailPrice,
                        ResellerPrice = _catalogUtilities.CalculateResellerPrice(productCatalogModel.RetailPrice, productCatalogModel.ResellerPrice)
                    });
                }
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task RemoveProductItemFromCatalog(int companyId, int productItemId, int catalogId)
        {
            var company = await _dbContext.Companies
                .Include(c => c.MyCompanies.Select(mc => mc.CompanyCatalogs))
                .Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems))
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var companyCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogId == catalogId && c.CatalogType == CatalogType.MyCatalog);

            if (companyCatalog == null)
                throw new NonExistingCompanyCatalogException($"CompanyId: {companyId}, CatalogI: {catalogId}");

            var catalogProductItem = companyCatalog.Catalog.CatalogProductItems.FirstOrDefault(p => p.ProductItemId == productItemId);

            if (catalogProductItem != null)
                _dbContext.CatalogProductItems.Remove(catalogProductItem);

            foreach (var myCompany in company.MyCompanies.Where(c => c.CompanyCatalogs.Any(cc => cc.CatalogType == CatalogType.Assigned && cc.CatalogId == catalogId)))
            {
                foreach (var myCompanyCatalog in myCompany.CompanyCatalogs.Where(c => c.CatalogType == CatalogType.MyCatalog))
                {
                    await RemoveProductItemFromCatalog(myCompany.Id, productItemId, myCompanyCatalog.CatalogId);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task ChangeRetailPrice(int companyId, int productItemId, int catalogId, decimal newRetailPrice)
        {
            var company = await _dbContext.Companies.Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems)).Include(c => c.MyCompanies.Select(mc => mc.CompanyCatalogs)).FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var companyCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogId == catalogId);

            if (companyCatalog == null)
                throw new NonExistingCompanyCatalogException($"CompanyId: {company.Id}, CatalogI: {catalogId}");

            var catalogProductItem = companyCatalog.Catalog.CatalogProductItems.FirstOrDefault(cp => cp.ProductItemId == productItemId);

            if (catalogProductItem == null)
                throw new NullReferenceException($"No product with id {productItemId}");

            catalogProductItem.RetailPrice = newRetailPrice;

            if (catalogProductItem.FixedRetailPrice)
            {
                foreach (var myCompany in company.MyCompanies.Where(c => c.CompanyCatalogs.Any(cc => cc.CatalogType == CatalogType.Assigned && cc.CatalogId == catalogId)))
                {
                    foreach (var myCompanyCatalog in myCompany.CompanyCatalogs.Where(c => c.CatalogType == CatalogType.MyCatalog))
                    {
                        await ChangeRetailPrice(myCompany.Id, productItemId, myCompanyCatalog.CatalogId, newRetailPrice);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task ChangeProductAvailability(int companyId, int catalogId, int productItemId, bool available)
        {
            var company = await _dbContext.Companies
                .Include(c => c.MyCompanies.Select(mc => mc.CompanyCatalogs))
                .Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems)).FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var selectedCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogId == catalogId && c.CatalogType == CatalogType.MyCatalog);

            if (selectedCatalog == null)
                throw new NonExistingCompanyCatalogException($"CompanyId: {company.Id}, CatalogI: {catalogId}");

            var selectedProductItem = selectedCatalog.Catalog.CatalogProductItems.FirstOrDefault(p => p.ProductItemId == productItemId);

            if (selectedProductItem == null)
                throw new Exception($"There is no product {productItemId} in catalog {catalogId} for company {companyId}");

            selectedProductItem.Available = available;

            await _dbContext.SaveChangesAsync();

            foreach (var myCompany in company.MyCompanies.Where(c => c.CompanyCatalogs.Any(cc => cc.CatalogType == CatalogType.Assigned && cc.CatalogId == catalogId)))
            {
                foreach (var myCompanyCatalog in myCompany.CompanyCatalogs.Where(cc => cc.CatalogType == CatalogType.MyCatalog))
                {
                    if (!available)
                        await RemoveProductItemFromCatalog(myCompany.Id, productItemId, myCompanyCatalog.CatalogId);
                    else
                        await AddProductItemToCatalog(new CatalogProductModel
                        {
                            CatalogId = myCompanyCatalog.CatalogId,
                            CompanyId = myCompany.Id,
                            ProductItemId = productItemId,
                            ResellerPrice = _catalogUtilities.CalculateResellerPrice(selectedProductItem.RetailPrice, selectedProductItem.ResellerPrice),
                            RetailPrice = selectedProductItem.RetailPrice,
                            FixedRetailPrice = selectedProductItem.FixedRetailPrice
                        });
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}