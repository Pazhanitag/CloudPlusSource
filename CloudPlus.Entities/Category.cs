using System;
using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Entities
{
    public class Category : IBaseEntity
    {
        public Category()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        [Required]
        public string Name { get; set; }

        public int Ord { get; set; }
        public Category Parent { get; set; }
        public string Vendor { get; set; }
        public string ImgUrl { get; set; }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}