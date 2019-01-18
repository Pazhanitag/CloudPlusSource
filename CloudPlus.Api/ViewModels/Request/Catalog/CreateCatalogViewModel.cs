using System.Collections.Generic;

namespace CloudPlus.Api.ViewModels.Request.Catalog
{
    public class CreateCatalogViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SourceCatalogId { get; set; }
        public IEnumerable<int> CompaniesAssignedToCatalog { get; set; }
    }
}