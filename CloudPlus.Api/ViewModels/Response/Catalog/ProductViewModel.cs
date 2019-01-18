using System.Collections.Generic;

namespace CloudPlus.Api.ViewModels.Response.Catalog
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string Vendor { get; set; }
        public IEnumerable<ProductItemViewModel> ProductItems { get; set; }
    }
}