namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendDatabasesubscription
{
    public interface ISuspendDatabasesubscriptionLog
    {
        string Office365SubscriptionId { get; set; }
        int Quantity { get; set; }
    }
}