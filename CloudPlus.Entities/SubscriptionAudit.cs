using System;

namespace CloudPlus.Entities
{
    public class SubscriptionAudit : IBaseEntity
    {
        public SubscriptionAudit()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public int OldQuantity { get; set; }

        public int NewQuantity { get; set; }

        public Subscription Subscription { get; set; }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}