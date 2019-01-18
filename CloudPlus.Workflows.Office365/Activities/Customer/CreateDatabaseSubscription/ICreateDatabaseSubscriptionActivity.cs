using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseSubscription
{
    public interface ICreateDatabaseSubscriptionActivity : Activity<ICreateDatabaseSubscriptionArguments,
        ICreateDatabaseSubscriptionLog>
    {

    }
}