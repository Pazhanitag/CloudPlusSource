namespace CloudPlus.Api.ViewModels.Response.Catalog
{
    public class ProductItemViewModel
    {
        public int ProductItemId { get; set; }
        public string Name { get; set; }
        public decimal ResellerPrice { get; set; }
        public decimal Cost { get; set; }
        public decimal RetailPrice { get; set; }
        public bool FixedRetailPrice { get; set; }
        public bool Available { get; set; }
        public bool IsAddon { get; set; }
    }
}