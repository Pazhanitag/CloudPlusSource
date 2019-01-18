using System.Linq;
using CloudPlus.Enums.User;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Office365.Transition.Commands;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public class ActivityOffice365TransitionUserAndLicensesArgumentsMapper
        : IActivityOffice365TransitionUserAndLicensesArgumentsMapper
    {
        public dynamic MapCreateAdUserArguments(IOffice365TransitionUserAndLicensesCommand src)
        {
            return new
            {
                src.CompanyId,
                src.UserPrincipalName,
                src.FirstName,
                src.LastName,
                src.DisplayName,
                PhoneNumber = "",
                Address = "",
                City = "",
                State = "",
                Zip = "",
                Country = "US",
                JobTitle = "",
                Company = "",
                EmailAddress = src.UserPrincipalName,
                Email = src.UserPrincipalName,
                src.Password,
                Upn = src.UserPrincipalName,
                CompanyDomain = src.UserPrincipalName.Split('@').LastOrDefault(),
                WorkflowActivityType = WorkflowActivityType.Office365TransitionUserAndLicenses,
                WorkflowStep = WorkflowActivityStep.CreateActiveDirectoryUser
            };
        }

        public dynamic MapCreateIsUserArguments(IOffice365TransitionUserAndLicensesCommand src)
        {
            return new
            {
                src.CompanyId,
                src.UserPrincipalName,
                src.FirstName,
                src.LastName,
                src.DisplayName,
                UserName = src.UserPrincipalName,
                PhoneNumber = "",
                City = "",
                State = "",
                ZipCode = "",
                Country = "",
                Department = "",
                CountryCode = "US",
                JobTitle = "",
                Company = "",
                EmailAddress = src.UserPrincipalName,
                Email = src.UserPrincipalName,
                src.Password,
                UserStatus = UserStatus.Active,
                Upn = src.UserPrincipalName,
                StreetAddress = "",
                src.Roles,
                WorkflowActivityType = WorkflowActivityType.Office365TransitionUserAndLicenses,
                WorkflowStep = WorkflowActivityStep.CreateLocalUser
            };
        }

        public dynamic MapCreateOffice365DatabaseUserArguments(IOffice365TransitionUserAndLicensesCommand src)
        {
            return new
            {
                src.CompanyId,
                src.UserPrincipalName,
                src.Office365UserId,
                WorkflowActivityType = WorkflowActivityType.Office365TransitionUserAndLicenses,
                WorkflowStep = WorkflowActivityStep.Office365CreateDatabaseUser
            };
        }

        public dynamic MapGetUserRolesArguments(IOffice365TransitionUserAndLicensesCommand src)
        {
            return new
            {
                src.CompanyId,
                src.UserPrincipalName,
                WorkflowActivityType = WorkflowActivityType.Office365TransitionUserAndLicenses,
                WorkflowStep = WorkflowActivityStep.Office365GetUserRoles
            };
        }

        public dynamic MapRemoveUserRolesArguments(IOffice365TransitionUserAndLicensesCommand src)
        {
            return new
            {
                src.CompanyId,
                src.UserPrincipalName,
                src.Office365CustomerId,
                WorkflowActivityType = WorkflowActivityType.Office365TransitionUserAndLicenses,
                WorkflowStep = WorkflowActivityStep.Office365RemoveUserRoles
            };
        }

        public dynamic MapAssignUserRolesArguments(IOffice365TransitionUserAndLicensesCommand src)
        {
            return new
            {
                src.CompanyId,
                src.UserPrincipalName,
                src.Office365CustomerId,
                src.UserRoles,
                WorkflowActivityType = WorkflowActivityType.Office365TransitionUserAndLicenses,
                WorkflowStep = WorkflowActivityStep.Office365AssignUserRoles
            };
        }

        public dynamic MapRemoveAllLicensesPartnerPortalUserArguments(IOffice365TransitionUserAndLicensesCommand src)
        {
            return new
            {
                src.CompanyId,
                src.UserPrincipalName,
                src.Office365UserId,
                src.Office365CustomerId,
                WorkflowActivityType = WorkflowActivityType.Office365TransitionUserAndLicenses,
                WorkflowStep = WorkflowActivityStep.Office365RemoveAllLicensePartnerPortalUser
            };
        }

        public dynamic MapAssignLicenseOffice365PartnerPlatformUserArguments(IOffice365TransitionUserAndLicensesCommand src)
        {
            return new
            {
                src.CompanyId,
                src.Office365CustomerId,
                src.UserPrincipalName,
                src.CloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365TransitionUserAndLicenses,
                WorkflowStep = WorkflowActivityStep.Office365AssignLicenseToPartnerPlatformUser
            };
        }

        public dynamic MapAssignLicenseOffice365DatabaseUserArguments(IOffice365TransitionUserAndLicensesCommand src)
        {
            return new
            {
                src.CompanyId,
                src.UserPrincipalName,
                src.CloudPlusProductIdentifier,
                WorkflowActivityType = WorkflowActivityType.Office365TransitionUserAndLicenses,
                WorkflowStep = WorkflowActivityStep.Office365AssignLicenseToDatabaseUser
            };
        }
    }
}
