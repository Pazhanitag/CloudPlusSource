using System.Collections.Generic;
using CloudPlus.Models.Office365.Subscription;

namespace CloudPlus.Workflows.Office365.Activities.Customer.MultiDatabaseCustomerSubscription
{
    public class MultiDatabaseCustomerSubscriptionLog : IMultiDatabaseCustomerSubscriptionLog
    {
        public IEnumerable<Office365SubscriptionModel> Subscriptions { get; set; }
    }
}
