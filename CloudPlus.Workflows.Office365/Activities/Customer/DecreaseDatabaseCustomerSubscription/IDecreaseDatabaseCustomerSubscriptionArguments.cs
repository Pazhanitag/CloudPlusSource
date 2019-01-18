namespace CloudPlus.Workflows.Office365.Activities.Customer.DecreaseDatabaseCustomerSubscription
{
    public interface IDecreaseDatabaseCustomerSubscriptionArguments
    {
        string Office365CustomerId { get; set; }
        string CloudPlusProductIdentifier { get; set; }
        string Office365SubscriptionId { get; set; }
    }
}
