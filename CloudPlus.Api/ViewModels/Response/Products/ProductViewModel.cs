using System.Collections.Generic;

namespace CloudPlus.Api.ViewModels.Response.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public int Ord { get; set; }
        public string Vendor { get; set; }
        public string Description { get; set; }
        public ProductCategoryViewModel Category { get; set; }
        public IEnumerable<ProductItemViewModel> ProductItems { get; set; }
    }
}
