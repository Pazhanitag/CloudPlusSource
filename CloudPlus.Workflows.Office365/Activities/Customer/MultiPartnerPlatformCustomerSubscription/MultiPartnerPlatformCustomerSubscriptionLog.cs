using System.Collections.Generic;
using CloudPlus.Models.Office365.Subscription;

namespace CloudPlus.Workflows.Office365.Activities.Customer.MultiPartnerPlatformCustomerSubscription
{
    public class MultiPartnerPlatformCustomerSubscriptionLog : IMultiPartnerPlatformCustomerSubscriptionLog
    {
        public IEnumerable<Office365SubscriptionModel> Subscriptions { get; set; }
    }
}
