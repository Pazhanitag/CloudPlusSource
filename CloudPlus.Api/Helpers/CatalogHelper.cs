using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Api.Mappers;
using CloudPlus.Api.ViewModels.Request.Catalog;
using CloudPlus.Api.ViewModels.Response.Catalog;
using CloudPlus.Models.Catalog;
using CloudPlus.Services.Database.Catalog;
using CloudPlus.Services.Identity.Permission;
using OfficeOpenXml;

namespace CloudPlus.Api.Helpers
{
    public class CatalogHelper : ICatalogHelper
    {
        private readonly ICompanyCatalogService _companyCatalogService;
        private readonly IPermissionService _permissionService;
        private readonly ICatalogProductItemService _catalogProductItemService;

        public CatalogHelper(ICompanyCatalogService companyCatalogService, IPermissionService permissionService, ICatalogProductItemService catalogProductItemService)
        {
            _companyCatalogService = companyCatalogService;
            _permissionService = permissionService;
            _catalogProductItemService = catalogProductItemService;
        }

        public async Task<IEnumerable<CatalogViewModel>> GetCatalogs(int companyId)
        {
            var catalogs = await _companyCatalogService.GetCompanyCatalogs(companyId);

            var catalogsViewModels = catalogs.OrderByDescending(c => c.Default).ThenBy(c => c.Name).Select(c => c.ToResellerViewModel()).ToList();

            foreach (var catalogViewModel in catalogsViewModels)
            {
                var companiesAssignedToCatalog = await _companyCatalogService.GetCompaniesAssignedToCatalog(companyId, catalogViewModel.Id);

                catalogViewModel.CompaniesAssignedToCatalog = companiesAssignedToCatalog.Select(c => c.ToCatalogCompanyViewModel());
            }
            return catalogsViewModels;
        }

        public async Task<CatalogViewModel> GetCatalog(int companyId, int catalogId)
        {
            var catalog = await _companyCatalogService.GetCompanyCatalog(companyId, catalogId);
            var catalogViewModel = catalog.ToResellerViewModel();

            var myCompaniesAssignedToCatalog = await _companyCatalogService.GetCompaniesAssignedToCatalog(companyId, catalogId);
            catalogViewModel.CompaniesAssignedToCatalog = myCompaniesAssignedToCatalog.Select(c => c.ToCatalogCompanyViewModel());

            return catalogViewModel;
        }

        public async Task UpdateFixedRetailPrice(int userId, int companyId, ResellerProductItemModel currentModel, UpdateProductItemViewModel newModel, int catalogId)
        {
            var permissions = _permissionService.GetUserPermissions(userId).ToList();

            if (currentModel.FixedRetailPrice != newModel.FixedRetailPrice)
            {
                if (permissions.Any(p => p.Name == "SetMsrpFixed"))
                {
                    await _catalogProductItemService.UpdateFixedRetailPrice(companyId, newModel.ProductItemId, catalogId, newModel.FixedRetailPrice);
                }
            }
        }

        public async Task UpdateRetailPrice(int userId, int companyId, ResellerProductItemModel currentModel, UpdateProductItemViewModel newModel, int catalogId)
        {
            var permissions = _permissionService.GetUserPermissions(userId).ToList();

            if (currentModel.RetailPrice != newModel.RetailPrice)
            {
                if (permissions.Any(p => p.Name == "SetMsrpFixed") || !currentModel.FixedRetailPrice)
                {
                    await _catalogProductItemService.ChangeRetailPrice(companyId, newModel.ProductItemId, catalogId, newModel.RetailPrice);
                }
            }
        }

        public async Task UpdateResellerPrice(int companyId, ResellerProductItemModel currentModel, UpdateProductItemViewModel newModel, int catalogId)
        {
            if (currentModel.ResellerPrice != newModel.ResellerPrice)
            {
                await _catalogProductItemService.ChangeResellerPrice(companyId, newModel.ProductItemId, catalogId, newModel.ResellerPrice);
            }
        }
        #region Get product details for download the product details
        //TAG Dev
        public async Task<Tuple<MemoryStream, String>> GetProductDetailsAsMemoryStream(int companyId, int catalogId)
        {
            DataTable dtAllCatalog = new DataTable();
            var Product = await _companyCatalogService.GetProductDownloadData(companyId, catalogId);
            System.IO.MemoryStream MemoryStreamSheet = ProductDataTableToMemoryStream(Product.Item1, "Sheet1");
            MemoryStreamSheet.Position = 0;
            return Tuple.Create(MemoryStreamSheet, Product.Item2);
        }
        #endregion

        #region Convert product details datat table into memory stream 
        //TAG Dev
        public static MemoryStream ProductDataTableToMemoryStream(System.Data.DataTable table, string sheetName)
        {
            MemoryStream productStream = new MemoryStream();
            ExcelPackage productExcelPack = new ExcelPackage();
            ExcelWorksheet productWorkSheet = productExcelPack.Workbook.Worksheets.Add(sheetName);

            int column = 1;
            int row = 1;

            for (int i = 0; i < table.Columns.Count; i++)
            {
                productWorkSheet.Cells[row, column].Value = table.Columns[i].ColumnName.ToString();
                column++;
            }
            column = 1;
            row++;
            foreach (DataRow currentRow in table.Rows)
            {
                foreach (DataColumn currentColumn in table.Columns)
                {
                    if (currentRow[currentColumn.ColumnName] != DBNull.Value)
                        productWorkSheet.Cells[row, column].Value = currentRow[currentColumn.ColumnName].ToString();
                    column++;
                }
                row++;
                column = 1;
            }
            productExcelPack.SaveAs(productStream);

            return productStream;
        }
        #endregion
    }
}