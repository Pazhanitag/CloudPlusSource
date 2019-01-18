namespace CloudPlus.Models.Catalog
{
    public class ResellerProductItemModel
    {
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public decimal Cost { get; set; }
        public decimal ResellerPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public bool FixedRetailPrice { get; set; }
        public bool Available { get; set; }
        public bool IsAddon { get; set; }
        public int Ord { get; set; }
        public string CatalogName { get; set; }
    }

    public class ResellerProductItemEmailModel
    {
        public string ProductName { get; set; }
        public decimal Cost { get; set; }
        public decimal ResellerPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public bool FixedRetailPrice { get; set; }
    }

}