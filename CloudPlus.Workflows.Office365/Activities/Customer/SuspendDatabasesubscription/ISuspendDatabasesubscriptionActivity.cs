using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendDatabasesubscription
{
    public interface
        ISuspendDatabasesubscriptionActivity : Activity<ISuspendDatabasesubscriptionArguments,
            ISuspendDatabasesubscriptionLog>
    {

    }
}