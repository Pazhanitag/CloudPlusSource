namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseSubscription
{
    public class CreateDatabaseSubscriptionLog : ICreateDatabaseSubscriptionLog
    {
        public string Office365CustomerId { get; set; }
        public string CloudPlusProductIdentifier { get; set; }
    }
}