namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdatePartnerPlatformSubscriptionQuantity
{
    public class UpdatePartnerPlatformSubscriptionQuantityLog : IUpdatePartnerPlatformSubscriptionQuantityLog
    {
        public string Office365SubscriptionId { get; set; }
        public string Office365CustomerId { get; set; }
        public int QuantityChange { get; set; }
    }
}