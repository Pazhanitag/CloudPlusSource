using System;
using System.Collections.Generic;
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
    public class CustomerCatalogService : ICustomerCatalogService
    {
        private readonly CldpDbContext _dbContext;

        public CustomerCatalogService(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CustomerProductModel>> GetCustomerProducts(int companyId)
        {
            var company = await _dbContext.Companies
                .Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems.Select(cp => cp.ProductItem.Product)))
                .FirstOrDefaultAsync(c => c.Id == companyId);


            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");
            var assignedCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogType == CatalogType.Assigned);
            

            if (assignedCatalog == null)
                throw new NoAssignedCatalogException($"CompanyId: {companyId}");

            var productItems = assignedCatalog.Catalog.CatalogProductItems.Where(p => p.Available).Select(p => new CustomerProductItemModel
            {
                ProductId = p.ProductItem.Product.Id,
                ProductItemId = p.ProductItemId,
                Name = p.ProductItem.Name,
                Description = p.ProductItem.Description,
                IsAddon = p.ProductItem.IsAddon,
                Identifier = p.ProductItem.Identifier,
                Cost = company.Type == 1 ? p.RetailPrice : p.ResellerPrice
            });
           // productItems = company.Type == 1? productItems.Where(x => x.Name != "(#CustomerName#) Support").ToList(): productItems;

            var distinctProducts = assignedCatalog.Catalog.CatalogProductItems.Select(p => p.ProductItem.Product).Distinct();
            //distinctProducts= company.Type == 1 ? distinctProducts.Where(x => x.Name != "(#CustomerName#) Support") : distinctProducts;
            IEnumerable<CustomerProductModel> lstCustomerproduct = new List<CustomerProductModel>();

            lstCustomerproduct = distinctProducts.Select(distinctProduct => new CustomerProductModel
            {
                activatedDate = _dbContext.Provisions.Where(x => x.IsDeleted == false && x.CompanyId == companyId && x.ProductV2Id == distinctProduct.Id).Select(x => x.CreateDate).SingleOrDefault(),
                ProductId = distinctProduct.Id,
                Name = (distinctProduct.Name).Replace("(#CustomerName#)", company.Name),
                Description = distinctProduct.Description,
                Vendor = distinctProduct.Vendor,
                Img = distinctProduct.ImgUrl,
                CategoryId = distinctProduct.GroupId,
                ProductItems = productItems.Where(productItem => productItem.ProductId == distinctProduct.Id ).OrderBy(x => x.Name).ToList(),
            });
            
            return lstCustomerproduct;
        }

        public async Task<CustomerProductModel> GetCustomerProduct(int companyId, int productId)
        {
            var company = await _dbContext.Companies
                .Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems.Select(cp => cp.ProductItem.Product)))
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var assignedCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogType == CatalogType.Assigned);

            if (assignedCatalog == null)
                throw new NoAssignedCatalogException($"CompanyId: {companyId}");

            var productItems = assignedCatalog.Catalog.CatalogProductItems.Where(p => p.Available && p.ProductItem.Product.Id == productId).Select(p => new CustomerProductItemModel
            {
                ProductId = p.ProductItem.Product.Id,
                ProductItemId = p.ProductItemId,
                Name = p.ProductItem.Name,
                Description = p.ProductItem.Description,
                IsAddon = p.ProductItem.IsAddon,
                Cost = company.Type == 1 ? p.RetailPrice : p.ResellerPrice,
                Identifier = p.ProductItem.Identifier
            });

            var product = assignedCatalog.Catalog.CatalogProductItems.FirstOrDefault(p => p.ProductItem.Product != null && p.ProductItem.Product.Id == productId)?.ProductItem.Product;

            if (product == null)
                throw new Exception($"Non existing product for company {companyId} product {productId}");

            return new CustomerProductModel
            {
                ProductId = product.Id,
                Name = product.Name,
                Vendor = product.Vendor,
                Img = product.ImgUrl,
                ProductItems = productItems.ToList()
            };
        }
    }
}