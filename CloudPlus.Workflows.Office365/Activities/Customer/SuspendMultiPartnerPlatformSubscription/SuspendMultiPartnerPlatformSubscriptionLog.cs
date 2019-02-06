using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendMultiPartnerPlatformSubscription
{
    public class SuspendMultiPartnerPlatformSubscriptionLog : ISuspendMultiPartnerPlatformSubscriptionLog
    {
        public string Office365CustomerId { get; set; }
        public List<string> Office365SubscriptionIds { get; set; }
    }
}
