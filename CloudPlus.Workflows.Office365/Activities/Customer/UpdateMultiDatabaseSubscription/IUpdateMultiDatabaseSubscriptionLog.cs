
using CloudPlus.Models.Office365.Subscription;
using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscription
{
    public interface IUpdateMultiDatabaseSubscriptionLog
    {
        List<Office365SubscriptionModel> office365SubscriptionModels { get; set; }
        string Office365OrderId { get; set; }
        string Office365CustomerId { get; set; }
    }
}
