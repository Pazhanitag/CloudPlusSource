using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Customer.ActivateMultiSuspendedPartnerPlatformSubscription
{
    public class ActivateMultiSuspendedPartnerPlatformSubscriptionLog : IActivateMultiSuspendedPartnerPlatformSubscriptionLog
    {
        public List<string> Office365SubscriptionIds { get; set; }
        public string Office365CustomerId { get; set; }
    }
}
