using System.Collections.Generic;
using CloudPlus.Services.Database.Product;

namespace CloudPlus.Services.Database.ProductItem
{
    public interface IProductItemService
    {
        IEnumerable<ProductItemModel> GetProductItems();
    }
}
