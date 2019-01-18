using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.ActivateSoftDeletedDatabaseUser
{
    public interface IActivateSoftDeletedDatabaseUserActivity : Activity<IActivateSoftDeletedDatabaseUserArguments, IActivateSoftDeletedDatabaseUserLog>
    {
    }
}
