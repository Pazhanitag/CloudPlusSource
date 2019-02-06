using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Customer.ActivateMultiSuspendedPartnerPlatformSubscription
{
    public interface IActivateMultiSuspendedPartnerPlatformSubscriptionArguments
    {
        List<string> Office365SubscriptionIds { get; set; }
        string Office365CustomerId { get; set; }
    }
}
