
using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Customer.ActivateMultiSuspendedPartnerPlatformSubscription
{
    public interface IActivateMultiSuspendedPartnerPlatformSubscriptionLog
    {
        List<string> Office365SubscriptionIds { get; set; }
        string Office365CustomerId { get; set; }
    }
}
