using CloudPlus.Models.Office365.Subscription;
using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscription
{
    public class UpdateMultiDatabaseSubscriptionLog: IUpdateMultiDatabaseSubscriptionLog
    {
        public List<Office365SubscriptionModel> office365SubscriptionModels { get; set; }
        public string Office365OrderId { get; set; }
        public string Office365CustomerId { get; set; }
    }
}
