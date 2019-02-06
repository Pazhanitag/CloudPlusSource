using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendMultiPartnerPlatformSubscription
{
    public interface ISuspendMultiPartnerPlatformSubscriptionArguments
    {
        string Office365CustomerId { get; set; }
        List<string> Office365SubscriptionIds { get; set; }
    }
}
