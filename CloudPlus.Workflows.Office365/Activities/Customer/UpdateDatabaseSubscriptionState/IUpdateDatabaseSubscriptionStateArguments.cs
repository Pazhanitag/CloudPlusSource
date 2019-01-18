using CloudPlus.Enums.Office365;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscriptionState
{
    public interface IUpdateDatabaseSubscriptionStateArguments
    {
        Office365SubscriptionState SubscriptionState { get; set; }
        string Office365SubscriptionId { get; set; }
    }
}