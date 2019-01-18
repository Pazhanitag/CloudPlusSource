namespace CloudPlus.Workflows.Office365.Activities.Customer.DecreaseDatabaseCustomerSubscription
{
    public interface IDecreaseDatabaseCustomerSubscriptionLog
    {
        string Office365CustomerId { get; set; }
        string CloudPlusProductIdentifier { get; set; }
        string Office365SubscriptionId { get; set; }
        string Office365OrderId { get; set; }
        string Office365FriendlyName { get; set; }
    }
}
