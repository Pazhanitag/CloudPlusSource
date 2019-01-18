using System;

namespace CloudPlus.Entities.Office365
{
    public class Office365License : IBaseEntity
    {
        public Office365License()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public Office365User Office365User { get; set; }
        public Office365Offer Office365Offer { get; set; }
    }
}
