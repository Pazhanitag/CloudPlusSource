using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.ActivateMultiSuspendedDatabaseSubscription
{
    public class ActivateMultiSuspendedDatabaseSubscriptionLog : IActivateMultiSuspendedDatabaseSubscriptionLog
    {
        public List<string> Office365SubscriptionIds { get; set; }
    }
}
