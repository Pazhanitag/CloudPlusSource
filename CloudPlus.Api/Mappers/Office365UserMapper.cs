using System.Collections.Generic;
using CloudPlus.Api.ViewModels.Request.Office365;
using CloudPlus.QueueModels.Office365.Subscriptions.Commands;

namespace CloudPlus.Api.Mappers
{
    public static class Office365UserMapper
    {
        public static dynamic ToManageSubscriptionsAndLicencesCommand(this Office365UserMultiAddViewModel viewModel)
        {
            return new
            {
                viewModel.CompanyId,
                viewModel.CloudPlusProductIdentifiers,
                viewModel.Users,
                viewModel.UserRoles,
                viewModel.SecurityGroupName,
                viewModel.DistributionGroupName,
                viewModel.Office365GroupName,
                MessageType = ManageSubsctiptionAndLicenceCommandType.MultiAddUser
            };
        }
        public static dynamic ToManageSubscriptionsAndLicencesCommand(this Office365UserRestoreViewModel viewModel)
        {
            return new
            {
                viewModel.CompanyId,
                Users = new List<Office365UserViewModel> { new Office365UserViewModel { UserPrincipalName = viewModel.UserPrincipalName } },
                MessageType = ManageSubsctiptionAndLicenceCommandType.RestoreUser
            };
        }
        public static dynamic ToManageSubscriptionsAndLicencesCommand(this Office365UserRemoveLicenseViewModel viewModel)
        {
            return new
            {
                viewModel.CompanyId,
                Users = new List<Office365UserViewModel> { new Office365UserViewModel { UserPrincipalName = viewModel.UserPrincipalName } },
                MessageType = ManageSubsctiptionAndLicenceCommandType.RemoveLicence
            };
        }
        public static dynamic ToManageSubscriptionsAndLicencesCommand(this Office365UserChangeLicenseViewModel viewModel)
        {
            return new
            {
                viewModel.CompanyId,
                CloudPlusProductIdentifier = viewModel.AssignCloudPlusProductIdentifier,
                Users = new List<Office365UserViewModel> { new Office365UserViewModel { UserPrincipalName = viewModel.UserPrincipalName } },
                viewModel.UserRoles,
                MessageType = ManageSubsctiptionAndLicenceCommandType.ChangeLicence
            };
        }

        public static dynamic ToManageSubscriptionsAndLicencesCommand(this Office365UserAssignLicenseViewModel viewModel)
        {
            return new
            {
                viewModel.CompanyId,
                viewModel.CloudPlusProductIdentifier,
                Users = new List<Office365UserViewModel>
                {
                    new Office365UserViewModel
                    {
                        UserPrincipalName = viewModel.UserPrincipalName,
                        Password = string.IsNullOrEmpty(viewModel.Password) ? null : viewModel.Password
                    }
                },
                viewModel.UserRoles,
                MessageType = ManageSubsctiptionAndLicenceCommandType.AssignNewLicence
            };
        }

        public static dynamic ToOffice365UserChangeRolesCommand(this Office365UserChangeLicenseViewModel viewModel)
        {
            return new
            {
                viewModel.CompanyId,
                viewModel.UserPrincipalName,
                viewModel.UserRoles
            };
        }

        //TAG
        public static dynamic ToManageSubscriptionsAndLicencesCommand(this Office365UserEditViewModel viewModel)
        {
            return new
            {
                viewModel.CompanyId,
                viewModel.CloudPlusProductIdentifiers,
                viewModel.User,
                viewModel.UserRoles,
                viewModel.SecurityGroupName,
                viewModel.DistributionGroupName,
                viewModel.Office365GroupName,
                MessageType = ManageSubsctiptionAndLicenceCommandType.EditUser
            };
        }

    }
}
