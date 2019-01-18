namespace CloudPlus.Api.ViewModels.Request.Catalog
{
    public class UpdateProductItemViewModel
    {
        public int ProductItemId { get; set; }
        public bool Available { get; set; }
        public bool FixedRetailPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal ResellerPrice { get; set; }
    }
}