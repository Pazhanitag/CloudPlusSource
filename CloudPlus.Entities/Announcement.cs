using System;
using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Entities
{
    public class Announcement : IBaseEntity
    {
        public Announcement()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime? PublishDate { get; set; }
        public DateTime? FinishDate { get; set; }

        public int Id { get; set; }


        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}