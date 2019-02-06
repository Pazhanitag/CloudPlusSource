using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.ActivateMultiSuspendedDatabaseSubscription
{
    public interface IActivateMultiSuspendedDatabaseSubscriptionArguments
    {
        List<string> Office365SubscriptionIds { get; set; }
    }
}
