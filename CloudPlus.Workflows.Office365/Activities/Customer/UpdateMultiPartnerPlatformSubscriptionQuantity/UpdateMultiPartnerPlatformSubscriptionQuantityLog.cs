using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiPartnerPlatformSubscriptionQuantity
{
    public class UpdateMultiPartnerPlatformSubscriptionQuantityLog: IUpdateMultiPartnerPlatformSubscriptionQuantityLog
    {
        public Dictionary<string,int> Office365SubscriptionIds { get; set; }
        public string Office365CustomerId { get; set; }
    }
}
