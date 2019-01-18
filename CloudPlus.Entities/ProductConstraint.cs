using System;
using System.ComponentModel.DataAnnotations;
using CloudPlus.Entities.Catalog;

namespace CloudPlus.Entities
{
    public class ProductConstraint : IBaseEntity
    {
        public ProductConstraint()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public Product Product { get; set; }

        [MaxLength]
        public string Constraint { get; set; }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}