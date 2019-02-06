using CloudPlus.Enums.Office365;
using CloudPlus.Models.Office365.Offer;
using System.Collections.Generic;

namespace CloudPlus.Models.Office365.Subscription
{
    public class Office365OrderSubscriptionModel
    {
        public string OrderId { get; set; }
        public List<Office365SubscriptionModel> office365SubscriptionModels { get; set; }
    }
    public class Office365SubscriptionModel
    {
        public string Office365CustomerId { get; set; }
        public string Office365SubscriptionId { get; set; }
        public string Office365OrderId { get; set; }
        public string Office365FriendlyName { get; set; }
        public int Quantity { get; set; }
        public Office365OfferModel Office365Offer { get; set; }
        public Office365SubscriptionState SubscriptionState { get; set; }
        public bool Suspended { get; set; }
    }
}
