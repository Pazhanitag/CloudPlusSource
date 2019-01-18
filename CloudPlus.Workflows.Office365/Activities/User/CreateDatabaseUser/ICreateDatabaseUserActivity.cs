using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.CreateDatabaseUser
{
    public interface ICreateDatabaseUserActivity : Activity<ICreateDatabaseUserArguments, ICreateDatabaseUserLog>
    {
    }
}
