using System.Linq;
using CloudPlus.Api.ViewModels.Response.Catalog;
using CloudPlus.Models.Catalog;

namespace CloudPlus.Api.Mappers
{
    public static class CatalogMapper
    {
        public static CatalogViewModel ToResellerViewModel(this CatalogModel resellerCatalog)
        {
            return new CatalogViewModel
            {
                Id = resellerCatalog.Id,
                Name = resellerCatalog.Name,
                Description = resellerCatalog.Description,
                Default = resellerCatalog.Default,
                CreateDate = resellerCatalog.CreateDate,
                Products = resellerCatalog.Products?.Select(p => p.ToResellerCatalogProductViewModel()),
            };
        }

        public static CatalogCompanyViewModel ToCatalogCompanyViewModel(this CatalogCompanyModel catalogCompany)
        {
            return new CatalogCompanyViewModel
            {
                CompanyId = catalogCompany.CompanyId,
                CompanyName = catalogCompany.CompanyName,
                CatalogId = catalogCompany.CatalogId,
                CatalogName = catalogCompany.CatalogName
            };
        }
        public static ProductViewModel ToResellerCatalogProductViewModel(this ResellerProductModel resellerCatalogProduct)
        {
            return new ProductViewModel
            {
                Id = resellerCatalogProduct.Id,
                Name = resellerCatalogProduct.Name,
                Img = resellerCatalogProduct.Img,
                Vendor = resellerCatalogProduct.Vendor,
                ProductItems = resellerCatalogProduct.ProductItems.Select(p => p.ToResellerCatalogProductItemViewModel())
            };
        }

        public static ProductItemViewModel ToResellerCatalogProductItemViewModel(
            this ResellerProductItemModel resellerCatalogProductItem)
        {
            return new ProductItemViewModel
            {
                ProductItemId = resellerCatalogProductItem.ProductItemId,
                Name = resellerCatalogProductItem.ProductName,
                FixedRetailPrice = resellerCatalogProductItem.FixedRetailPrice,
                RetailPrice = resellerCatalogProductItem.RetailPrice,
                ResellerPrice = resellerCatalogProductItem.ResellerPrice,
                Cost = resellerCatalogProductItem.Cost,
                IsAddon = resellerCatalogProductItem.IsAddon,
                Available = resellerCatalogProductItem.Available
            };
        }
        public static CustomerProductViewModel ToCustomerCatalogProductViewModel(
            this CustomerProductModel customerCatalogProduct, string imageServerPath)
        {
            //return new CustomerProductViewModel
            //{
            //    Id = customerCatalogProduct.ProductId,
            //    Name = customerCatalogProduct.Name,
            //    Description = customerCatalogProduct.Description,
            //    Vendor = customerCatalogProduct.Vendor,
            //    ImgUrl = customerCatalogProduct.Img,
            //    activatedDate = customerCatalogProduct.activatedDate,
            //    ProductItems = customerCatalogProduct.ProductItems.Select(c => c.ToCustomerCatalogProductItemViewModel(imageServerPath))
            //};
            return new CustomerProductViewModel
            {
                Id = customerCatalogProduct.ProductId,
                Name = customerCatalogProduct.Name,
                Description = customerCatalogProduct.Description,
                Vendor = customerCatalogProduct.Vendor,
                ImgUrl = customerCatalogProduct.Img,
                activatedDate = customerCatalogProduct.activatedDate,
               CategoryId = customerCatalogProduct.CategoryId,
                ProductItems = customerCatalogProduct.ProductItems.Select(c => c.ToCustomerCatalogProductItemViewModel(imageServerPath))
            };
        }

        public static CustomerProductItemViewModel ToCustomerCatalogProductItemViewModel(
            this CustomerProductItemModel customerProductItemModel, string imageServerPath)
        {
            const string imageServerOffice365IconsPathPlaceholder = "(#ImageServerOffice365IconsPath#)";

            return new CustomerProductItemViewModel
            {
                Id = customerProductItemModel.ProductItemId,
                IsAddon = customerProductItemModel.IsAddon,
                Name = customerProductItemModel.Name,
                Description = customerProductItemModel.Description.Replace(imageServerOffice365IconsPathPlaceholder, imageServerPath),
                Cost = customerProductItemModel.Cost,
                Identifier = customerProductItemModel.Identifier
            };
        }
    }
}