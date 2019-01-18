using System;
using System.Collections.Generic;

namespace CloudPlus.Api.ViewModels.Response.Catalog
{
    public class CustomerProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Vendor { get; set; }
        public string ImgUrl { get; set; }
        public int CategoryId { get; set; }
        public DateTime activatedDate { get; set; }
        public IEnumerable<CustomerProductItemViewModel> ProductItems { get; set; }
    }
}