using System.Collections.Generic;
using CloudPlus.Models.Office365.Subscription;

namespace CloudPlus.Workflows.Office365.Activities.Customer.MultiPartnerPlatformCustomerSubscription
{
    public interface IMultiPartnerPlatformCustomerSubscriptionLog
    {
        IEnumerable<Office365SubscriptionModel> Subscriptions { get; set; }
    }
}
