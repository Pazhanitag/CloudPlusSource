using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.RemoveUserRoles
{
    public interface IRemoveUserRolesActivity : Activity<IRemoveUserRolesArguments, IRemoveUserRolesLog>
    {
    }
}
