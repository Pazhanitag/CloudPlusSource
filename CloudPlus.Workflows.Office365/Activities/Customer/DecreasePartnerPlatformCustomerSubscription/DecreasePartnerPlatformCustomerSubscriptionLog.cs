namespace CloudPlus.Workflows.Office365.Activities.Customer.DecreasePartnerPlatformCustomerSubscription
{
    public class DecreasePartnerPlatformCustomerSubscriptionLog : IDecreasePartnerPlatformCustomerSubscriptionLog
    {
        public string Office365CustomerId { get; set; }
        public string Office365SubscriptionId { get; set; }
        public int Quantity { get; set; }
    }
}
