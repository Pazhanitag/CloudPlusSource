using CloudPlus.Enums.Office365;
using CloudPlus.Models.Office365.Offer;

namespace CloudPlus.Models.Office365.Subscription
{
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
