using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Office365.User.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public class ActivityOffice365UserChangeRolesMapper : IActivityOffice365UserChangeRolesMapper
    {
        public dynamic MapGetUserRolesArguments(IOffice365UserChangeRolesCommand src)
        {
            return new
            {
                src.CompanyId,
                src.UserPrincipalName,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeRoles,
                WorkflowStep = WorkflowActivityStep.Office365GetUserRoles
            };
        }

        public dynamic MapRemoveUserRolesArguments(IOffice365UserChangeRolesCommand src)
        {
            return new
            {
                src.UserPrincipalName,
                src.UserRoles,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeRoles,
                WorkflowStep = WorkflowActivityStep.Office365RemoveUserRoles
            };
        }

        public dynamic MapAssignUserRolesArguments(IOffice365UserChangeRolesCommand src)
        {
            return new
            {
                src.UserPrincipalName,
                src.UserRoles,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeRoles,
                WorkflowStep = WorkflowActivityStep.Office365AssignUserRoles
            };
        }
    }
}
