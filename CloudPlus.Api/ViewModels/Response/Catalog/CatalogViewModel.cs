using System;
using System.Collections.Generic;

namespace CloudPlus.Api.ViewModels.Response.Catalog
{
    public class CatalogViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
        public IEnumerable<CatalogCompanyViewModel> CompaniesAssignedToCatalog { get; set; }

        public bool Default { get; set; }
        public string Description { get; set; }
    }
}