using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Office365.User.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public class ActivityOffice365UserArgumentsMapper : IActivityOffice365UserArgumentsMapper
    {
        public dynamic MapCreatePartnerPlatformUserArguments(IOffice365UserAssignLicenseCommand src)
        {
            var dest = new
            {
                src.CompanyId,
                src.UserPrincipalName,
                src.DisplayName,
                src.FirstName,
                src.LastName,
                src.UsageLocation,
                src.City,
                src.Country,
                src.PhoneNumber,
                src.PostalCode,
                src.State,
                src.StreetAddress,
                src.Password,
                WorkflowActivityType = WorkflowActivityType.Office365UserAssignLicense,
                WorkflowStep = WorkflowActivityStep.Office365CreatePartnerPlatformUser
            };

            return dest;
        }

        public dynamic MapCreateOffice365DatabaseUserArguments(IOffice365UserAssignLicenseCommand src)
        {
            var dest = new
            {
                src.UserPrincipalName,
                WorkflowActivityType = WorkflowActivityType.Office365UserAssignLicense,
                WorkflowStep = WorkflowActivityStep.Office365CreateDatabaseUser
            };

            return dest;
        }

        public dynamic MapPartnerPlatformOffice365CustomerSubscriptionArguments(IOffice365UserAssignLicenseCommand src)
        {
            var dest = new
            {
                src.CompanyId,
                src.UserPrincipalName,
                src.CloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365UserAssignLicense,
                WorkflowStep = WorkflowActivityStep.Office365PartnerPlatformCustomerSubscription
            };

            return dest;
        }

        public dynamic MapDatabaseOffice365CustomerSubscriptionArguments(IOffice365UserAssignLicenseCommand src)
        {
            var dest = new
            {
                src.UserPrincipalName,
                src.CloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365UserAssignLicense,
                WorkflowStep = WorkflowActivityStep.Office365DatabaseCustomerSubscription
            };

            return dest;
        }

        public dynamic MapUpdateDatabaseOffice365CustomerSubscriptionArguments(IOffice365UserAssignLicenseCommand src)
        {
            return new
            {
                src.UserPrincipalName,
                src.CloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365UserAssignLicense,
                WorkflowStep = WorkflowActivityStep.Office365UpdateDatabaseCustomerSubscription
            };
        }

        public dynamic MapAssignLicenseOffice365PartnerPlatformUserArguments(IOffice365UserAssignLicenseCommand src)
        {
            var dest = new
            {
                src.UserPrincipalName,
                src.CloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365UserAssignLicense,
                WorkflowStep = WorkflowActivityStep.Office365AssignLicenseToPartnerPlatformUser
            };

            return dest;
        }

        public dynamic MapAssignLicenseOffice365DatabaseUserArguments(IOffice365UserAssignLicenseCommand src)
        {
            var dest = new
            {
                src.UserPrincipalName,
                src.CloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365UserAssignLicense,
                WorkflowStep = WorkflowActivityStep.Office365AssignLicenseToDatabaseUser
            };

            return dest;
        }

        public dynamic MapAssignUserRolesArguments(IOffice365UserAssignLicenseCommand src)
        {
            var dest = new
            {
                src.UserPrincipalName,
                src.UserRoles,
                WorkflowActivityType = WorkflowActivityType.Office365UserAssignLicense,
                WorkflowStep = WorkflowActivityStep.Office365AssignUserRoles
            };

            return dest;
        }

        public dynamic MapSendOffice365UserSetupEmailArguments(IOffice365UserAssignLicenseCommand src)
        {
            var dest = new
            {
                src.UserPrincipalName,
                WorkflowActivityType = WorkflowActivityType.Office365UserAssignLicense,
                WorkflowStep = WorkflowActivityStep.Office365SendUserSetupEmail
            };

            return dest;
        }

        public dynamic MapDeletePartnerPlatformUserArguments(IOffice365UserRemoveLicenseCommand src)
        {
            var dest = new
            {
                src.CompanyId,
                src.UserPrincipalName,
                WorkflowActivityType = WorkflowActivityType.Office365UserRemoveLicense,
                WorkflowStep = WorkflowActivityStep.Office365DeletePartnerPlatformUser
            };

            return dest;
        }

        public dynamic MapSoftDeleteDatabaseUserArguments(IOffice365UserRemoveLicenseCommand src)
        {
            var dest = new
            {
                src.UserPrincipalName,
                WorkflowActivityType = WorkflowActivityType.Office365UserRemoveLicense,
                WorkflowStep = WorkflowActivityStep.Office365SoftDeleteDatabaseUser
            };

            return dest;
        }

        public dynamic MapDecreasePartnerPlatformCustomerSubscriptionArguments(IOffice365UserRemoveLicenseCommand src)
        {
            var dest = new
            {
                src.CompanyId,
                src.UserPrincipalName,
                WorkflowActivityType = WorkflowActivityType.Office365UserRemoveLicense,
                WorkflowStep = WorkflowActivityStep.Office365DecreasePartnerPlatformCustomerSubscription
            };

            return dest;
        }

        public dynamic MapDecreaseDatabaseCustomerSubscriptionArguments(IOffice365UserRemoveLicenseCommand src)
        {
            var dest = new
            {
                src.UserPrincipalName,
                WorkflowActivityType = WorkflowActivityType.Office365UserRemoveLicense,
                WorkflowStep = WorkflowActivityStep.Office365DecreaseDatabaseCustomerSubscription
            };

            return dest;
        }
    }
}
