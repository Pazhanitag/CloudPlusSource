using System.Linq;
using CloudPlus.Services.Database.Product;
using ProductItemViewModel = CloudPlus.Api.ViewModels.Response.Products.ProductItemViewModel;
using ProductViewModel = CloudPlus.Api.ViewModels.Response.Products.ProductViewModel;

namespace CloudPlus.Api.Mappers
{
    public static class ProductMapper
    {
        public static ProductViewModel ToProductViewModel(this ProductModel model, string rootUrl)
        {
            if (model == null) return null;

            const string imageServerOffice365IconsPathPlaceholder = "(#ImageServerOffice365IconsPath#)";
            var path = rootUrl + "/static/Images/Office365Icons";

            return new ProductViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Ord = model.Ord,
                Vendor = model.Vendor,
                ProductItems = model.ProductItems.Select(item => new ProductItemViewModel
                {
                    Id = item.Id,
                    Identifier = item.Identifier,
                    IsAddon = item.IsAddon,
                    Name = item.Name,
                    Description = item.Description.Replace(imageServerOffice365IconsPathPlaceholder, path),
                    Status = item.Status,
                    //Cost = item.Price // TODO
                }),
                ImgUrl = model.ImgUrl,
            };
        }
    }
}