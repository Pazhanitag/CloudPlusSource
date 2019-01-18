namespace CloudPlus.Workflows.Office365.Activities.Customer.DatabaseCustomerSubscription
{
    public class DatabaseCustomerSubscriptionLog : IDatabaseCustomerSubscriptionLog
    {
        public string CloudPlusProductIdentifier { get; set; }
        public int Quantity { get; set; }
        public string Office365CustomerId { get; set; }
    }
}
