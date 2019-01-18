using System.Collections.Generic;

namespace CloudPlus.Api.ViewModels.Request.Catalog
{
    public class UpdateCatalogViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<int> CompaniesAssignedToCatalog { get; set; }
        public IEnumerable<UpdateProductItemViewModel> ProductItems { get; set; }
    }
}