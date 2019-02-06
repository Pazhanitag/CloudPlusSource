using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscriptionQuantity
{
    public class UpdateMultiDatabaseSubscriptionQuantityLog : IUpdateMultiDatabaseSubscriptionQuantityLog
    {
        public Dictionary<string, int> Office365SubscriptionIds { get; set; }
    }
}
