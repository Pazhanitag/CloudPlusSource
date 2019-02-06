using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Office365.User.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public class ActivityOffice365UserChangeLicenseMapper : IActivityOffice365UserChangeLicenseMapper
    {
        public dynamic MapDecreasePartnerPlatformCustomerSubscriptionArguments(IOffice365UserChangeLicenseCommand src)
        {
            return new
            {
                src.CompanyId,
                src.Office365CustomerId,
                src.UserPrincipalName,
                CloudPlusProductIdentifier = src.RemoveCloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                WorkflowStep = WorkflowActivityStep.Office365DecreasePartnerPlatformCustomerSubscription
            };
        }

        public dynamic MapDecreaseDatabaseCustomerSubscriptionArguments(IOffice365UserChangeLicenseCommand src)
        {
            return new
            {
                src.UserPrincipalName,
                src.Office365CustomerId,
                CloudPlusProductIdentifier = src.RemoveCloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                WorkflowStep = WorkflowActivityStep.Office365DecreaseDatabaseCustomerSubscription
            };
        }

        public dynamic MapRemoveLicensePartnerPortalUserArguments(IOffice365UserChangeLicenseCommand src)
        {
            return new
            {
                src.UserPrincipalName,
                src.Office365CustomerId,
                CloudPlusProductIdentifier = src.RemoveCloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                WorkflowStep = WorkflowActivityStep.Office365RemoveLicensePartnerPortalUser
            };
        }

        public dynamic MapRemoveLicenseDatabaseUserArguments(IOffice365UserChangeLicenseCommand src)
        {
            return new
            {
                src.UserPrincipalName,
                src.Office365CustomerId,
                CloudPlusProductIdentifier = src.RemoveCloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                WorkflowStep = WorkflowActivityStep.Office365RemoveLicenseDatabaseUser
            };
        }

        public dynamic MapPartnerPlatformOffice365CustomerSubscriptionArguments(IOffice365UserChangeLicenseCommand src)
        {
            return new
            {
                src.CompanyId,
                src.UserPrincipalName,
                src.Office365CustomerId,
                CloudPlusProductIdentifier = src.AssignCloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                WorkflowStep = WorkflowActivityStep.Office365PartnerPlatformCustomerSubscription
            };
        }

        public dynamic MapDatabaseOffice365CustomerSubscriptionArguments(IOffice365UserChangeLicenseCommand src)
        {
            return new
            {
                src.UserPrincipalName,
                src.Office365CustomerId,
                CloudPlusProductIdentifier = src.AssignCloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                WorkflowStep = WorkflowActivityStep.Office365DatabaseCustomerSubscription
            };
        }

        public dynamic MapGetUserRolesArguments(IOffice365UserChangeLicenseCommand src)
        {
            return new
            {
                src.CompanyId,
                src.UserPrincipalName,
                src.Office365CustomerId,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                WorkflowStep = WorkflowActivityStep.Office365GetUserRoles
            };
        }

        public dynamic MapRemoveUserRolesArguments(IOffice365UserChangeLicenseCommand src)
        {
            return new
            {
                src.UserPrincipalName,
                src.Office365CustomerId,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                WorkflowStep = WorkflowActivityStep.Office365RemoveUserRoles
            };
        }

        public dynamic MapAssignUserRolesArguments(IOffice365UserChangeLicenseCommand src)
        {
            return new
            {
                src.UserRoles,
                src.Office365CustomerId,
                src.UserPrincipalName,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                WorkflowStep = WorkflowActivityStep.Office365AssignUserRoles
            };
        }

        public dynamic MapAssignLicenseOffice365PartnerPlatformUserArguments(IOffice365UserChangeLicenseCommand src)
        {
            return new
            {
                src.UserPrincipalName,
                src.Office365CustomerId,
                CloudPlusProductIdentifier = src.AssignCloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                WorkflowStep = WorkflowActivityStep.Office365AssignLicenseToPartnerPlatformUser
            };
        }

        public dynamic MapAssignLicenseOffice365DatabaseUserArguments(IOffice365UserChangeLicenseCommand src)
        {
            return new
            {
                src.UserPrincipalName,
                src.Office365CustomerId,
                CloudPlusProductIdentifier = src.AssignCloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                WorkflowStep = WorkflowActivityStep.Office365AssignLicenseToDatabaseUser
            };
        }
    }
}
