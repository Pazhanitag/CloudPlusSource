using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.PriceCatalog
{
    public class ClonePriceCatalogViewModel
    {
        [Required]
        public int CompanyId { get; set; }
    }
}