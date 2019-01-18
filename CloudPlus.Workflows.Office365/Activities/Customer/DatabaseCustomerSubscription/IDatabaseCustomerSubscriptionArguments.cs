namespace CloudPlus.Workflows.Office365.Activities.Customer.DatabaseCustomerSubscription
{
    public interface IDatabaseCustomerSubscriptionArguments
    {
        string CloudPlusProductIdentifier { get; set; }
        string Office365CustomerId { get; set; }
    }
}
