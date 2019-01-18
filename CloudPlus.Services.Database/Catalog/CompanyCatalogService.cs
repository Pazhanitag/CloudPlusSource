using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Entities.Catalog;
using CloudPlus.Exceptions.Catalog;
using CloudPlus.Exceptions.Company;
using CloudPlus.Models.Catalog;


namespace CloudPlus.Services.Database.Catalog
{
    public class CompanyCatalogService : ICompanyCatalogService
    {
        private readonly CldpDbContext _dbContext;
        private readonly ICatalogProductItemService _catalogProductItemService;
        private readonly ICatalogUtilities _catalogUtilities;

        public CompanyCatalogService(CldpDbContext dbContext, ICatalogProductItemService catalogProductItemService, ICatalogUtilities catalogUtilities)
        {
            _dbContext = dbContext;
            _catalogProductItemService = catalogProductItemService;
            _catalogUtilities = catalogUtilities;
        }

        public async Task<IEnumerable<CatalogCompanyModel>> GetCompaniesAssignedToCatalog(int companyId, int catalogId)
        {
            var company = await _dbContext.Companies.Include(c => c.MyCompanies.Select(mc => mc.CompanyCatalogs))
                .Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog)).FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var selectedCatalog = company.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.MyCatalog && cc.CatalogId == catalogId);

            if (selectedCatalog == null)
                throw new NonExistingCompanyCatalogException($"CompanyId {companyId}, CatalogId {catalogId}");

            return company.MyCompanies.Where(mc => mc.CompanyCatalogs.Any(cc => cc.CatalogType == CatalogType.Assigned && cc.CatalogId == catalogId))
                .Select(mc => new CatalogCompanyModel
                {
                    CatalogId = selectedCatalog.CatalogId,
                    CatalogName = selectedCatalog.Catalog.Name,
                    CompanyId = mc.Id,
                    CompanyName = mc.Name
                });
        }
        public async Task<CatalogModel> GetCompanyAssignedCatalog(int companyId)
        {
            var company = await _dbContext.Companies.Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog)).FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var assignedCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogType == CatalogType.Assigned);

            if (assignedCatalog == null)
                throw new NoAssignedCatalogException($"CompanyId: {companyId}");

            return new CatalogModel
            {
                Id = assignedCatalog.Catalog.Id,
                Name = assignedCatalog.Catalog.Name,
                Description = assignedCatalog.Catalog.Description
            };
        }
        public async Task AssignDefaultCatalogToCompany(int parentCompanyId, int companyId)
        {
            var parentCompany = await _dbContext.Companies
                .Include(c => c.MyCompanies.Select(cc => cc.CompanyCatalogs))
                .Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog))
                .FirstOrDefaultAsync(c => c.Id == parentCompanyId);

            if (parentCompany == null)
                throw new CompanyNotFoundException($"Parent CompanyId {parentCompanyId}");

            var company = parentCompany.MyCompanies.FirstOrDefault(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId {companyId}");

            var defaultCatalog = parentCompany.CompanyCatalogs.FirstOrDefault(cc => cc.Default);

            if (defaultCatalog == null)
                throw new NonExistingCompanyCatalogException($"Companyid {parentCompanyId}");

            var assignedCatalog = company.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.Assigned);

            if (assignedCatalog != null)
                _dbContext.CompanyCatalogs.Remove(assignedCatalog);

            _dbContext.CompanyCatalogs.Add(new CompanyCatalog
            {
                Catalog = defaultCatalog.Catalog,
                Company = company,
                CatalogType = CatalogType.Assigned
            });

            await _dbContext.SaveChangesAsync();
        }
        public async Task AssignCatalogToCompany(int parentCompanyId, int companyId, int catalogId)
        {
            var parentCompany = await _dbContext.Companies
                .Include(c => c.MyCompanies.Select(mc => mc.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems)))
                .Include(c => c.MyCompanies.Select(mc => mc.MyCompanies.Select(cc => cc.CompanyCatalogs.Select(mcc => mcc.Catalog.CatalogProductItems))))
                .Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems)).FirstOrDefaultAsync(c => c.Id == parentCompanyId);

            if (parentCompany == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var company = parentCompany.MyCompanies.FirstOrDefault(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var assignedCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogType == CatalogType.Assigned);

            if (assignedCatalog != null)
            {
                if (assignedCatalog.CatalogId == catalogId)
                    return;

                _dbContext.CompanyCatalogs.Remove(assignedCatalog);
            }
            var catalogToAssign = parentCompany.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.MyCatalog && cc.CatalogId == catalogId);

            if (catalogToAssign == null)
                throw new NonExistingCompanyCatalogException($"CompanyId {parentCompanyId}, CatalogId {catalogId}");

            _dbContext.CompanyCatalogs.Add(new CompanyCatalog
            {
                CatalogType = CatalogType.Assigned,
                Catalog = catalogToAssign.Catalog,
                Company = company
            });

            foreach (var myCompanyCatalog in company.CompanyCatalogs.Where(cc => cc.CatalogType == CatalogType.MyCatalog))
            {
                foreach (var assignedCatalogProductItem in catalogToAssign.Catalog.CatalogProductItems)
                {
                    var myCompanyCatalogProductItem = myCompanyCatalog.Catalog.CatalogProductItems.FirstOrDefault(cc => cc.ProductItemId == assignedCatalogProductItem.ProductItemId);

                    if (myCompanyCatalogProductItem == null && assignedCatalogProductItem.Available)
                    {
                        await _catalogProductItemService.AddProductItemToCatalog(new CatalogProductModel
                        {
                            CatalogId = myCompanyCatalog.CatalogId,
                            CompanyId = myCompanyCatalog.CompanyId,
                            ProductItemId = assignedCatalogProductItem.ProductItemId,
                            RetailPrice = assignedCatalogProductItem.RetailPrice,
                            FixedRetailPrice = assignedCatalogProductItem.FixedRetailPrice,
                            ResellerPrice = _catalogUtilities.CalculateResellerPrice(assignedCatalogProductItem.RetailPrice, assignedCatalogProductItem.ResellerPrice)
                        });
                    }
                    else if (myCompanyCatalogProductItem != null && !assignedCatalogProductItem.Available)
                    {
                        await _catalogProductItemService.RemoveProductItemFromCatalog(company.Id, assignedCatalogProductItem.ProductItemId, myCompanyCatalog.CatalogId);
                    }

                    await _catalogProductItemService.UpdateFixedRetailPrice(company.Id, assignedCatalogProductItem.ProductItemId, myCompanyCatalog.CatalogId, assignedCatalogProductItem.FixedRetailPrice);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<CatalogModel>> GetCompanyCatalogs(int companyId)
        {
            var company = await _dbContext.Companies.Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog)).FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var myCatalogs = company.CompanyCatalogs.Where(c => c.CatalogType == CatalogType.MyCatalog);

            return myCatalogs.Select(companyCatalog => new CatalogModel
            {
                Id = companyCatalog.Catalog.Id,
                Name = companyCatalog.Catalog.Name,
                Description = companyCatalog.Catalog.Description,
                Default = companyCatalog.Default,
                CreateDate = companyCatalog.CreateDate
            });
        }
        public async Task DeleteCatalog(int companyId, int catalogId)
        {
            var company = await _dbContext.Companies
                .Include(c => c.MyCompanies.Select(mc => mc.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems)))
                .Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems.Select(cpi => cpi.ProductItem)))
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var companyCatalog = company.CompanyCatalogs.FirstOrDefault(cc =>
                cc.CatalogType == CatalogType.MyCatalog && cc.CatalogId == catalogId);

            if (companyCatalog == null)
                throw new NonExistingCompanyCatalogException($"CompanyId {companyId}, CatalogId {catalogId}");


            if (companyCatalog.Default)
            {
                var newDefaultCatalog = company.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.MyCatalog && cc.CatalogId != catalogId);

                if (newDefaultCatalog == null)
                    throw new Exception("You cannot delete last catalog");

                newDefaultCatalog.Default = true;
                companyCatalog.Default = false;
            }

            foreach (var myCompany in company.MyCompanies.Where(c => c.CompanyCatalogs.Any(cc => cc.CatalogType == CatalogType.Assigned && cc.CatalogId == catalogId)))
            {
                var defaultCatalog = company.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.MyCatalog && cc.Default);

                if (defaultCatalog == null)
                    throw new Exception();

                await AssignCatalogToCompany(company.Id, myCompany.Id, defaultCatalog.CatalogId);
            }

            _dbContext.Catalogs.Remove(companyCatalog.Catalog);
            _dbContext.CompanyCatalogs.Remove(companyCatalog);

            await _dbContext.SaveChangesAsync();
        }
        public async Task CreateNewCatalog(int companyId, int sourceCatalogId, CatalogModel resellerCatalogModel)
        {
            var company = await _dbContext.Companies.Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems.Select(cpi => cpi.ProductItem))).FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var sourceCompanyCatalog = company.CompanyCatalogs.FirstOrDefault(cc =>
                cc.CatalogType == CatalogType.MyCatalog && cc.CatalogId == sourceCatalogId);

            if (sourceCompanyCatalog == null)
                throw new NonExistingCompanyCatalogException($"CompanyId {companyId}, CatalogId {sourceCatalogId}");

            var newCatalog = new Entities.Catalog.Catalog
            {
                Name = resellerCatalogModel.Name,
                Description = resellerCatalogModel.Description
            };

            sourceCompanyCatalog.Catalog.CatalogProductItems.ForEach(productItem =>
            {
                newCatalog.CatalogProductItems.Add(new CatalogProductItem
                {
                    Catalog = newCatalog,
                    ProductItem = productItem.ProductItem,
                    RetailPrice = productItem.RetailPrice,
                    ResellerPrice = productItem.ResellerPrice,
                    FixedRetailPrice = productItem.FixedRetailPrice,
                    Available = productItem.Available
                });
            });

            _dbContext.Catalogs.Add(newCatalog);
            _dbContext.CompanyCatalogs.Add(new CompanyCatalog
            {
                Catalog = newCatalog,
                Company = company,
                CatalogType = CatalogType.MyCatalog
            });

            foreach (var assignedCompanies in resellerCatalogModel.CompaniesAssignedToCatalog)
            {
                await AssignCatalogToCompany(companyId, assignedCompanies.CompanyId, newCatalog.Id);
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateCompanyCatalog(int companyId, int catalogId, CatalogModel resellerCatalogModel)
        {
            var company = await _dbContext.Companies.Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog))
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var myCatalog = company.CompanyCatalogs.FirstOrDefault(cc =>
                cc.CatalogType == CatalogType.MyCatalog && cc.CatalogId == catalogId);

            if (myCatalog == null)
                throw new NonExistingCompanyCatalogException($"CompanyId {companyId}, CatalogId {catalogId}");

            myCatalog.Catalog.Name = resellerCatalogModel.Name;
            myCatalog.Catalog.Description = resellerCatalogModel.Description;

            await _dbContext.SaveChangesAsync();
        }
        public async Task<CatalogModel> GetCompanyCatalog(int companyId, int catalogId)
        {
            var company = await _dbContext.Companies.Include(c => c.Parent)
                .Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems.Select(cp => cp.ProductItem.Product))).FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var companyCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogId == catalogId && c.CatalogType == CatalogType.MyCatalog);

            if (companyCatalog == null)
                throw new NonExistingCompanyCatalogException($"CompanyId: {companyId}, CatalogI: {catalogId}");

            var assignedCatalog = company.CompanyCatalogs.FirstOrDefault(c => c.CatalogType == CatalogType.Assigned);

            if (assignedCatalog == null)
                throw new NoAssignedCatalogException($"Company Id {companyId}");

            var allProductItems = companyCatalog.Catalog.CatalogProductItems.Select(myCatalogProduct =>
            {
                var assignedCatalogProduct =
                    assignedCatalog.Catalog.CatalogProductItems.FirstOrDefault(c =>
                        c.ProductItemId == myCatalogProduct.ProductItemId);

                if (assignedCatalogProduct == null)
                    throw new NoProductInProductCatalogException(
                        $"CompanyId: {companyId}, CatalogId: {assignedCatalog.Catalog.Id}, ProductId: {myCatalogProduct.ProductItemId}");

                return new ResellerProductItemModel
                {
                    ProductId = myCatalogProduct.ProductItem.Product.Id,
                    ProductItemId = myCatalogProduct.ProductItemId,
                    Cost = assignedCatalogProduct.ResellerPrice,
                    ResellerPrice = myCatalogProduct.ResellerPrice,
                    RetailPrice = myCatalogProduct.RetailPrice,
                    ProductName = myCatalogProduct.ProductItem.Name,
                    FixedRetailPrice = myCatalogProduct.FixedRetailPrice,
                    IsAddon = myCatalogProduct.ProductItem.IsAddon,
                    Ord = myCatalogProduct.ProductItem.Ord,
                    Available = myCatalogProduct.Available,
                    CatalogName = myCatalogProduct.Catalog.Name,
                };
            }).OrderBy(p => p.ProductName);

            var catalog = new CatalogModel
            {
                Id = companyCatalog.CatalogId,
                Name = companyCatalog.Catalog.Name,
                Default = companyCatalog.Default,
                Description = companyCatalog.Catalog.Description,
                CreateDate = company.CreateDate,
                Products = GroupProductItems(allProductItems, company.Name)
            };
                return catalog;
        }
        public async Task RemoveCompanyFromAssignedCatalog(int parentCompanyId, int companyId)
        {
            var parentCompany = await _dbContext.Companies
                .Include(c => c.MyCompanies.Select(mc => mc.CompanyCatalogs))
                .Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog)).FirstOrDefaultAsync(c => c.Id == parentCompanyId);

            if (parentCompany == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var company = parentCompany.MyCompanies.FirstOrDefault(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var parentDefaultCatalog = parentCompany.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.MyCatalog && cc.Default);

            if (parentDefaultCatalog == null)
                throw new NonExistingCompanyCatalogException($"CompanyId {parentCompanyId}");

            await AssignCatalogToCompany(parentCompanyId, companyId, parentDefaultCatalog.CatalogId);
        }
        public async Task ChangeDefaultCompanyCatalog(int companyId, int newDefaultCatalogId)
        {
            var company = await _dbContext.Companies.Include(c => c.CompanyCatalogs).FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var newDefaultCatalog = company.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogId == newDefaultCatalogId);

            if (newDefaultCatalog == null)
                throw new NonExistingCompanyCatalogException($"CompanyId: {companyId}, CatalogI: {newDefaultCatalogId}");

            company.CompanyCatalogs.Where(cc => cc.CatalogType == CatalogType.MyCatalog).ToList().ForEach(cc => cc.Default = false);

            newDefaultCatalog.Default = true;

            await _dbContext.SaveChangesAsync();
        }
        public async Task GenerateDefaultCompanyCatalog(int companyId)
        {
            var company = await _dbContext.Companies.Include(c => c.CompanyCatalogs.Select(cc => cc.Catalog.CatalogProductItems)).FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            if (company.CompanyCatalogs.Any(cc => cc.CatalogType == CatalogType.MyCatalog))
                throw new Exception($"Default catalog for company {companyId} already exists");

            var assignedCompanyCatalog = company.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.Assigned);

            if (assignedCompanyCatalog == null)
                throw new NoAssignedCatalogException($"CompanyId: {companyId}");

            var newCatalog = new Entities.Catalog.Catalog
            {
                Name = "Default Price Schedule"
            };

            foreach (var catalogProduct in assignedCompanyCatalog.Catalog.CatalogProductItems.Where(p => p.Available))
            {
                newCatalog.CatalogProductItems.Add(new CatalogProductItem
                {
                    FixedRetailPrice = catalogProduct.FixedRetailPrice,
                    RetailPrice = catalogProduct.RetailPrice,
                    ProductItemId = catalogProduct.ProductItemId,
                    ResellerPrice = _catalogUtilities.CalculateResellerPrice(catalogProduct.RetailPrice, catalogProduct.ResellerPrice),
                    Available = catalogProduct.Available
                });
            }

            _dbContext.Catalogs.Add(newCatalog);

            company.CompanyCatalogs.Add(new CompanyCatalog
            {
                Catalog = newCatalog,
                CatalogType = CatalogType.MyCatalog,
                Default = true
            });

            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<CatalogCompanyModel>> GetMyCompaniesAssignedCatalogs(int companyId)
        {
            var company = await _dbContext.Companies.Include(c => c.MyCompanies.Select(mc => mc.CompanyCatalogs.Select(cc => cc.Catalog)))
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                throw new CompanyNotFoundException($"CompanyId: {companyId}");

            var myCompaniesAssignedCatalogs = new List<CatalogCompanyModel>();

            foreach (var myCompany in company.MyCompanies)
            {
                var assignedCatalog = myCompany.CompanyCatalogs.FirstOrDefault(cc => cc.CatalogType == CatalogType.Assigned);

                if (assignedCatalog == null)
                    throw new NoAssignedCatalogException($"CompanyId: {companyId}");

                myCompaniesAssignedCatalogs.Add(new CatalogCompanyModel
                {
                    CatalogId = assignedCatalog.Catalog.Id,
                    CatalogName = assignedCatalog.Catalog.Name,
                    CompanyId = myCompany.Id,
                    CompanyName = myCompany.Name
                });
            }
            return myCompaniesAssignedCatalogs;
        }

        private IEnumerable<ResellerProductModel> GroupProductItems(IEnumerable<ResellerProductItemModel> availableProductItems,string CompanyName)
        {
            var ret = new List<ResellerProductModel>();

            //foreach (var productItem in availableProductItems.OrderBy(p => p.Ord).ThenBy(p => p.IsAddon).ThenBy(p => p.ProductName))
            foreach (var productItem in availableProductItems.OrderBy(p => p.ProductName).ThenBy(p => p.Ord).ThenBy(p => p.IsAddon))
            {
                if (ret.Any(p => p.Id == productItem.ProductId))
                {
                    ret.FirstOrDefault(p => p.Id == productItem.ProductId)?.ProductItems.Add(productItem);
                }
                else
                {
                    var product = _dbContext.Products.FirstOrDefault(p => p.Id == productItem.ProductId);
                    if (product != null)
                    {

                        ret.Add(new ResellerProductModel
                        {
                            Id = product.Id,
                            Name = (product.Name).Replace("(#CustomerName#)", CompanyName),
                            Img = product.ImgUrl,
                            Vendor = product.Vendor,
                            ProductItems = new List<ResellerProductItemModel>
                            {
                                productItem
                            }
                        });
                    }
                }
            }
           

            return ret;
        }


        public async Task<DataTable> SendProductDetailsAsEmail(int companyId, int catalogId)
        {
            CatalogModel objCatalogModel = new CatalogModel();
            objCatalogModel = await GetCompanyCatalog(companyId, catalogId);
            DataTable dtAllCatalog = new DataTable();
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            foreach (var Product in objCatalogModel.Products)
            {
                var dty = converter.ToDataTable(Product.ProductItems, Product.Vendor + " " + Product.Name);
                dtAllCatalog.Merge(dty);
            }
            ///Removing  unwanted Column
            if (dtAllCatalog != null)
            {
                if (dtAllCatalog.Columns.Count > 0)
                {
                    dtAllCatalog = RemoveRenameProductTable(dtAllCatalog);
                }
            }
            //return Tuple.Create(dtAllCatalog, objCatalogModel.Name);
            return dtAllCatalog;
        }

        public class ListtoDataTableConverter
        {
            public DataTable ToDataTable<T>(IEnumerable<T> items, string VendorName)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                dataTable.Columns.Add("Vendor Name");
                foreach (PropertyInfo prop in Props)
                {
                    dataTable.Columns.Add(prop.Name);
                }

                foreach (T item in items)
                {
                    var values = new object[Props.Length + 1];
                    values[0] = VendorName;
                    for (int i = 0; i < Props.Length; i++)
                    {
                        values[i + 1] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }

                //table.SetColumnsOrder(new string[] { "Qty", "Unit", "Id" });
                return dataTable;

            }
        }

        private DataTable RemoveRenameProductTable(DataTable dtAllCatalog ) {
            if (dtAllCatalog != null)
            {
                if (dtAllCatalog.Columns.Count > 0)
                {
                    if (dtAllCatalog.Columns.Contains("ProductId"))
                        dtAllCatalog.Columns.Remove("ProductId");
                    if (dtAllCatalog.Columns.Contains("ProductItemId"))
                        dtAllCatalog.Columns.Remove("ProductItemId");
                    if (dtAllCatalog.Columns.Contains("FixedRetailPrice"))
                        dtAllCatalog.Columns.Remove("FixedRetailPrice");
                    if (dtAllCatalog.Columns.Contains("Available"))
                        dtAllCatalog.Columns.Remove("Available");
                    if (dtAllCatalog.Columns.Contains("IsAddon"))
                        dtAllCatalog.Columns.Remove("IsAddon");
                    if (dtAllCatalog.Columns.Contains("Ord"))
                        dtAllCatalog.Columns.Remove("Ord");
                    // Renaming the header
                    if (dtAllCatalog.Columns.Contains("ProductName"))
                        dtAllCatalog.Columns["ProductName"].ColumnName = "Product Name";
                    if (dtAllCatalog.Columns.Contains("ResellerPrice"))
                        dtAllCatalog.Columns["ResellerPrice"].ColumnName = "Reseller";
                    if (dtAllCatalog.Columns.Contains("RetailPrice"))
                        dtAllCatalog.Columns["RetailPrice"].ColumnName = "Customer";
                    if (dtAllCatalog.Columns.Contains("catalogName"))
                        dtAllCatalog.Columns["catalogName"].ColumnName = "Catalog Name";
                    if (dtAllCatalog.Columns.Contains("cost"))
                        dtAllCatalog.Columns["cost"].ColumnName = "Cost";
                }
            }
            return dtAllCatalog;
        }

        public async Task<Tuple<DataTable,String>> GetProductDownloadData(int companyId, int catalogId)
        {
            CatalogModel objCatalogModel = new CatalogModel();
            objCatalogModel = await GetCompanyCatalog(companyId, catalogId);
            DataTable dtAllCatalog = new DataTable();
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            foreach (var Product in objCatalogModel.Products)
            {
                var dty = converter.ToDataTable(Product.ProductItems, Product.Vendor + " " + Product.Name);
                dtAllCatalog.Merge(dty);
            }
            ///Removing  unwanted Column
            if (dtAllCatalog != null)
            {
                if (dtAllCatalog.Columns.Count > 0)
                {
                    dtAllCatalog = RemoveRenameProductTable(dtAllCatalog);
                }
            }
            return Tuple.Create(dtAllCatalog, objCatalogModel.Name);
            //return dtAllCatalog;
        }
    }
}