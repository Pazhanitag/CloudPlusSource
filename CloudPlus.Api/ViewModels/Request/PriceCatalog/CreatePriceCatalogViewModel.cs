using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.PriceCatalog
{
    public class CreatePriceCatalogViewModel
    {
        [Required]
        public decimal VendorCost { get; set; }
        [Required]
        public decimal ResellerPrice { get; set; }
        [Required]
        public decimal RetailPrice { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int CompanyId { get; set; }
    }
}