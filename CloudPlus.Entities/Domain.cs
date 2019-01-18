using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudPlus.Entities
{
    public class Domain : IBaseEntity
    {
        public Domain()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        [Required]
        public string Name { get; set; }
        public bool IsPrimary { get; set; }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        [Column("Company_Id")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
