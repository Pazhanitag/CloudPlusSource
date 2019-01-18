namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseSubscription
{
    public interface ICreateDatabaseSubscriptionArguments
    {
        string Office365CustomerId { get; set; }
        string CloudPlusProductIdentifier { get; set; }
        int Quantity { get; set; }
    }
}