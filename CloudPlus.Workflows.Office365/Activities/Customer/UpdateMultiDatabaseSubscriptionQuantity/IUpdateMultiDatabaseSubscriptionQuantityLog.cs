using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscriptionQuantity
{
    public interface IUpdateMultiDatabaseSubscriptionQuantityLog
    {
        Dictionary<string, int> Office365SubscriptionIds { get; set; }
    }
}
