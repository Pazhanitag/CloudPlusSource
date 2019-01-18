using System;

namespace CloudPlus.Entities.Catalog
{
    public class CatalogProductItem : IBaseEntity
    {
        public CatalogProductItem()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CatalogId { get; set; }
        public int ProductItemId { get; set; }

        public Entities.Catalog.Catalog Catalog { get; set; }
        public ProductItem ProductItem { get; set; }
        public bool Available { get; set; }
        public decimal ResellerPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public bool FixedRetailPrice { get; set; }
    }
}