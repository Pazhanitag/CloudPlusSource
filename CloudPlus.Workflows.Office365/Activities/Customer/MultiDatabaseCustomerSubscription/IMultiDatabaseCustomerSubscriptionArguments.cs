using System.Collections.Generic;
using CloudPlus.Models.Office365.Subscription;
using CloudPlus.Models.Office365.Transition;

namespace CloudPlus.Workflows.Office365.Activities.Customer.MultiDatabaseCustomerSubscription
{
    public interface IMultiDatabaseCustomerSubscriptionArguments
    {
        string Office365CustomerId { get; set; }
        Dictionary<string, int> CloudPlusProductIdentifiers { get; set; }
        IEnumerable<Office365TransitionProductItemModel> ProductItems { get; set; }
        IEnumerable<Office365SubscriptionModel> Subscriptions { get; set; }
    }
}
