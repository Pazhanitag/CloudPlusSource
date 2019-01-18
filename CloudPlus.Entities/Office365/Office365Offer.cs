using System;

namespace CloudPlus.Entities.Office365
{
    public class Office365Offer : IBaseEntity
    {
        public Office365Offer()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string Office365OfferId { get; set; }
        public string Office365OfferName { get; set; }
        public string Office365ProductSku { get; set; }
        public string CloudPlusProductIdentifier { get; set; }
    }
}
