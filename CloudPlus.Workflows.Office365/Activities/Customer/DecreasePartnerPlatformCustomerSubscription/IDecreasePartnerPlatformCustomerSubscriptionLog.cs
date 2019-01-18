namespace CloudPlus.Workflows.Office365.Activities.Customer.DecreasePartnerPlatformCustomerSubscription
{
    public interface IDecreasePartnerPlatformCustomerSubscriptionLog
    {
        string Office365CustomerId { get; set; }
        string Office365SubscriptionId { get; set; }
        int Quantity { get; set; }
    }
}
