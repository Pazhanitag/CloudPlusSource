using System;
using System.Collections.Generic;
using CloudPlus.Enums.Office365;

namespace CloudPlus.Entities.Office365
{
    public class Office365User : IBaseEntity
    {
        public Office365User()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
            Licenses = new List<Office365License>();
        }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string Office365UserId { get; set; }
        public int CloudPlusUserId { get; set; }
        public string UserPrincipalName { get; set; }
        public int CustomerId { get; set; }
        public Office365Customer Customer { get; set; }
        public List<Office365License> Licenses { get; set; }
        public Office365UserState Office365UserState { get; set; }
        public DateTime? Office365SoftDeletionTime { get; set; }
    }
}
