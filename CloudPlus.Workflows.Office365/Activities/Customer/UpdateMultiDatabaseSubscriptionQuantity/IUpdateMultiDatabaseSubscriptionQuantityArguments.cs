using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscriptionQuantity
{
    public interface IUpdateMultiDatabaseSubscriptionQuantityArguments
    {
        Dictionary<string,int> Office365SubscriptionIds { get; set; }
    }
}
