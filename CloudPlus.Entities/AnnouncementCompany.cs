using System;

namespace CloudPlus.Entities
{
    public class AnnouncementCompany : IBaseEntity
    {
        public AnnouncementCompany()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public Announcement Announcement { get; set; }
        public Company Company { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}