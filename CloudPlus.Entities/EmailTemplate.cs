using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Entities
{
    public class EmailTemplate : IBaseEntity
    {
        public EmailTemplate()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Template { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
