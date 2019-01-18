using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.RemoveLicenseDatabaseUser
{
    public interface IRemoveLicenseDatabaseUserActivity : Activity<IRemoveLicenseDatabaseUserArguments, IRemoveLicenseDatabaseUserLog>
    {
    }
}
