using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Office365.User.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public class ActivityOffice365UserRestoreMapper : IActivityOffice365UserRestoreMapper
    {
        public dynamic MapRestorePartnerPlatformUserArguments(IOffice365UserRestoreCommand command)
        {
            var dest = new
            {
                command.CompanyId,
                command.UserPrincipalName,
                WorkflowActivityType = WorkflowActivityType.Office365UserRestore,
                WorkflowStep = WorkflowActivityStep.Office365RestorePartnerPlatformUser
            };

            return dest;
        }

        public dynamic MapActivateSoftDeletedDatabaseUserArguments(IOffice365UserRestoreCommand command)
        {
            var dest = new
            {
                command.CompanyId,
                command.UserPrincipalName,
                WorkflowActivityType = WorkflowActivityType.Office365UserRestore,
                WorkflowStep = WorkflowActivityStep.Office365ActivateSoftDeletedDatabaseUser
            };

            return dest;
        }

        public dynamic MapPartnerPlatformOffice365CustomerSubscriptionArguments(IOffice365UserRestoreCommand command)
        {
            var dest = new
            {
                command.CompanyId,
                command.UserPrincipalName,
                WorkflowActivityType = WorkflowActivityType.Office365UserRestore,
                WorkflowStep = WorkflowActivityStep.Office365PartnerPlatformCustomerSubscription
            };

            return dest;
        }

        public dynamic MapDatabaseOffice365CustomerSubscriptionArguments(IOffice365UserRestoreCommand command)
        {
            var dest = new
            {
                command.UserPrincipalName,
                WorkflowActivityType = WorkflowActivityType.Office365UserRestore,
                WorkflowStep = WorkflowActivityStep.Office365DatabaseCustomerSubscription
            };

            return dest;
        }
    }
}
