using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.SoftDeleteDatabaseUser
{
    public interface ISoftDeleteDatabaseUserActivity : Activity<ISoftDeleteDatabaseUserArguments, ISoftDeleteDatabaseUserLog>
    {
    }
}
