using System;
using System.Collections.Generic;

namespace CloudPlus.Models.Catalog
{
    public class CustomerProductModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CustomerProductItemModel> ProductItems { get; set; }
        public string Vendor { get; set; }
        public string Img { get; set; }
        public DateTime activatedDate { get; set; }
        public int CategoryId { get; set; }
        public List<ServiceIdentifier> InCompatibleServices { get; set; }
    }
}