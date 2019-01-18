
namespace CloudPlus.Models.Price
{
    public class PriceCatalogModel
    {
        public int Id { get; set; }
        public decimal VendorCost { get; set; }
        public decimal ResellerPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int ProductId { get; set; }
        public int CompanyId { get; set; }
        public bool IsFixedPrice { get; set; }
        public bool IsAvailable { get; set; }
    }
}
