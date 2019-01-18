
namespace CloudPlus.Models.Price
{
    public class PriceWorkflowResultModel: PriceWorkflowModel
    {
        public decimal ResellerMarkupPrice { get; set; }
        public decimal ResellerProfitMarginPercentage { get; set; }
        public decimal ResellerProfitMarginPrice { get; set; }
        public decimal RetaiMarkupPrice { get; set; }
        public decimal RetailProfitMarginPercentage { get; set; }
        public decimal RetaiProfitMarginPrice { get; set; }
        public int ProductId { get; set; }
    }
}
