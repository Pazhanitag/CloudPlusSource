
namespace CloudPlus.Models.Price
{
    public class PriceWorkflowModel
    {
        public decimal VendorCost { get; set; }
        public decimal ResellerMarkupPercentage { get; set; }
        public decimal ResellerPrice { get; set; }
        public decimal RetailMarkupPercentage { get; set; }
        public decimal RetailPrice { get; set; }
        public bool IsFixedPrice { get; set; }
        public decimal DefaultPercentage { get; set; }
    }
}
