using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.PriceCatalog
{
    public class CalculatePriceViewModel
    {
        [Required]
        public decimal VendorCost { get; set; }
        [Required]
        public decimal ResellerPrice { get; set; }
        [Required]
        public decimal RetailPrice { get; set; }

        public bool IsFixedPrice { get; set; }
    }
}