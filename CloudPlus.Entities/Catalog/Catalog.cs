using System;
using System.Collections.Generic;

namespace CloudPlus.Entities.Catalog
{
    public class Catalog : IBaseEntity
    {
        public Catalog()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
            CatalogProductItems = new List<CatalogProductItem>();
        }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Name { get; set; }
        public List<CatalogProductItem> CatalogProductItems { get; set; }
        public string Description { get; set; }
    }
}