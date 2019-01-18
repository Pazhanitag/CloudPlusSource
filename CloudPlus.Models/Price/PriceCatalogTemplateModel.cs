using System;
using System.Collections.Generic;
using CloudPlus.Models.Company;

namespace CloudPlus.Models.Price
{
    public class PriceCatalogTemplateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Description { get; set; }
        public int CopyFromTemplateId { get; set; }
        public bool GlobalCatalog { get; set; }
        public IEnumerable<CompanyModel> Companies { get; set; }
    }
}
