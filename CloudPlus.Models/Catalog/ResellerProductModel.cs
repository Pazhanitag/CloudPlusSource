using System.Collections.Generic;

namespace CloudPlus.Models.Catalog
{
    public class ResellerProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string Vendor { get; set; }
        public List<ResellerProductItemModel> ProductItems { get; set; }
    }
   
}