using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.DatabaseCustomerSubscription
{
    public interface IDatabaseCustomerSubscriptionActivity : Activity<IDatabaseCustomerSubscriptionArguments, IDatabaseCustomerSubscriptionLog>
    {
    }
}
