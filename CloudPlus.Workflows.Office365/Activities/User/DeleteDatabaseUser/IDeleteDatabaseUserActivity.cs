using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.DeleteDatabaseUser
{
    public interface IDeleteDatabaseUserActivity : ExecuteActivity<IDeleteDatabaseUserArguments>
    {
    }
}
