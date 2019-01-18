namespace CloudPlus.Workflows.Office365.Activities.Customer.DatabaseCustomerSubscription
{
    public interface IDatabaseCustomerSubscriptionLog
    {
        string CloudPlusProductIdentifier { get; set; }
        int Quantity { get; set; }
        string Office365CustomerId { get; set; }
    }
}
