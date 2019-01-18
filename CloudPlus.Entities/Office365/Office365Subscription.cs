using System;
using CloudPlus.Enums.Office365;

namespace CloudPlus.Entities.Office365
{
    public class Office365Subscription : IBaseEntity
    {
        public Office365Subscription()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string Office365SubscriptionId { get; set; }
        public string Office365OrderId { get; set; }
        public string Office365FriendlyName { get; set; }
        public int Quantity { get; set; }

        public Office365Customer Office365Customer { get; set; }
        public Office365Offer Office365Offer { get; set; }

        public Office365SubscriptionState SubscriptionState { get; set; }
        public bool Suspended { get; set; }
    }
}
