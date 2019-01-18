using System;

namespace CloudPlus.Entities
{
    public class PublisherAnnouncment : IBaseEntity
    {
        public PublisherAnnouncment()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public int AnnouncementId { get; set; }

        public int PublisherId { get; set; }

        public bool AllowSupression { get; set; }

        public Announcement Announcement { get; set; }

        public Company Company { get; set; }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}