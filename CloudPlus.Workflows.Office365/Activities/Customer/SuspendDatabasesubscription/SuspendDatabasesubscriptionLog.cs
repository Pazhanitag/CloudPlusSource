namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendDatabasesubscription
{
    public class SuspendDatabasesubscriptionLog : ISuspendDatabasesubscriptionLog
    {
        public string Office365SubscriptionId { get; set; }
        public int Quantity { get; set; }
    }
}