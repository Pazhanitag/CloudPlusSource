using System;
using System.Collections.Generic;

namespace CloudPlus.Models.Catalog
{
    public class CatalogModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Default { get; set; }
        public DateTime CreateDate { get; set; }
        public IEnumerable<ResellerProductModel> Products { get; set; }
        public List<CatalogCompanyModel> CompaniesAssignedToCatalog { get; set; }
    }

    public class CatalogCompanyModel
    {
        public int CompanyId { get; set; }
        public int CatalogId { get; set; }
        public string CatalogName { get; set; }
        public string CompanyName { get; set; }
    }

   
}