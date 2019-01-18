namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseSubscription
{
    public interface ICreateDatabaseSubscriptionLog
    {
        string Office365CustomerId { get; set; }
        string CloudPlusProductIdentifier { get; set; }
    }
}