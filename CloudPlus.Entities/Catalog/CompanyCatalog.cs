using System;

namespace CloudPlus.Entities.Catalog
{
    public class CompanyCatalog : IBaseEntity
    {
        public CompanyCatalog()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CompanyId { get; set; }
        public int CatalogId { get; set; }
        public Company Company { get; set; }
        public Entities.Catalog.Catalog Catalog { get; set; }
        public CatalogType CatalogType { get; set; }
        public bool Default { get; set; }
    }

    public enum CatalogType
    {
        Assigned,
        MyCatalog
    }
}