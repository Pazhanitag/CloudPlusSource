using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.PriceCatalog
{
    public class EditPriceCatalogViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public decimal ResellPrice { get; set; }
        [Required]
        public decimal RetailPrice { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public bool Msrp { get; set; }
        [Required]
        public bool Available { get; set; }
    }
}