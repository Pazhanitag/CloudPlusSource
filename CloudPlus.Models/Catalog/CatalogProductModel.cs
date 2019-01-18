namespace CloudPlus.Models.Catalog
{
    public class CatalogProductModel
    {
        public int CatalogId { get; set; }
        public int ProductItemId { get; set; }
        public int CompanyId { get; set; }
        public decimal ResellerPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public bool FixedRetailPrice { get; set; }
    }
}