using System;
using CloudPlus.Entities.Catalog;

namespace CloudPlus.Entities
{
    public class CompanyProductExclusion : IBaseEntity
    {
        public CompanyProductExclusion()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}