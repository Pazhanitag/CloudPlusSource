using System;
using System.Collections.Generic;

namespace CloudPlus.Entities.Office365
{
    public class Office365Customer : IBaseEntity
    {
        public Office365Customer()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
            Office365Domains = new List<Office365Domain>();
            Office365Subscriptions = new List<Office365Subscription>();
            Office365Users = new List<Office365User>();
        }
        
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Office365Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public List<Office365Domain> Office365Domains { get; set; }
        public List<Office365Subscription> Office365Subscriptions { get; set; }
        public List<Office365User> Office365Users { get; set; }
    }
}
