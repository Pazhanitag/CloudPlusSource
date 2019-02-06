using CloudPlus.Enums.Office365;
using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscriptionState
{
    public interface IUpdateMultiDatabaseSubscriptionStateArguments
    {
        Office365SubscriptionState SubscriptionState { get; set; }
        List<string> Office365SubscriptionIds { get; set; }
    }
}
