using CloudPlus.QueueModels.Office365.User.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public interface IActivityOffice365UserChangeRolesMapper
    {
        dynamic MapGetUserRolesArguments(IOffice365UserChangeRolesCommand src);
        dynamic MapRemoveUserRolesArguments(IOffice365UserChangeRolesCommand src);
        dynamic MapAssignUserRolesArguments(IOffice365UserChangeRolesCommand src);
    }
}
