using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Constants;
using CloudPlus.Enums.User;
using CloudPlus.Models.Identity;
using CloudPlus.Models.Office365.Transition;
using CloudPlus.Models.Office365.User;
using CloudPlus.QueueModels.Office365.Transition.Commands;
using CloudPlus.QueueModels.Users.Commands;
using CloudPlus.Services.Identity.Role;
using CloudPlus.Services.Identity.User;
using CloudPlus.Services.Office365.User;

namespace CloudPlus.Workflows.Office365.Activities.Transition.TransitionDispatchCreatingUser
{
    public class TransitionDispatchCreatingUsersActivity : ITransitionDispatchCreatingUsersActivity
    {
        private readonly IUserService _userService;
        private readonly IOffice365UserService _office365UserService;
        private readonly IRoleService _roleService;

        public TransitionDispatchCreatingUsersActivity(
            IUserService userService,
            IOffice365UserService office365UserService,
            IRoleService roleService)
        {
            _userService = userService;
            _office365UserService = office365UserService;
            _roleService = roleService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ITransitionDispatchCreatingUsersArguments> context)
        {
            var arguments = context.Arguments;

            var users = _userService.GetUsers(arguments.CompanyId).ToList();
            var office365Users = await _office365UserService.GetAllOffice365Users(arguments.Office365CustomerId);
            var userRole = _roleService.GetAllRoles().FirstOrDefault(r => r.Name == "User");

            foreach (var item in arguments.ProductItems)
            {
                var user = users.FirstOrDefault(u => u.UserName == item.UserPrincipalName);
                var office365User = office365Users.FirstOrDefault(u => u.UserPrincipalName == item.UserPrincipalName);

                if (item.Delete)
                {
                    // Delete (soft delete) user from Office 365 portal
                    await SendDeletePartnerPlatformUserCommand(context, office365User);
                }
//                else if (item.KeepLicenses)
//                {                    
//                    if (user != null || office365User == null || userRole == null) continue;
//                    
//                    var sendEndpoint = await context.GetSendEndpoint(UserServiceConstants.CreateUserUri);
//                    await sendEndpoint.Send<ICreateUserCommand>(
//                        new
//                        {
//                            arguments.CompanyId,
//                            Email = office365User.UserPrincipalName,
//                            office365User.DisplayName,
//                            office365User.FirstName,
//                            office365User.LastName,
//                            item.Password,
//                            CountryCode = "US",
//                            UserStatus = UserStatus.Active,
//                            Roles = new List<int>{userRole.Id}
//                        }
//                    );
//                }
                else
                {
                    if (office365User == null) continue;
                    if (userRole == null) continue;

                    await SendTransitionUserAndLicensesCommand(context, office365User, user, userRole, item, item.KeepLicenses);
                }
            }

            return context.Completed();
        }

        private async Task SendDeletePartnerPlatformUserCommand(
            ExecuteContext<ITransitionDispatchCreatingUsersArguments> context, Office365UserModel office365User)
        {
            var arguments = context.Arguments;

            var deleteUserEndpoint = await context.GetSendEndpoint(Office365ServiceConstants.QueueOffice365UserAssignLicenseUri);

            await deleteUserEndpoint.Send<IOffice365TransitionDeletePartnerPlatformUserCommand>(new
            {
                arguments.Office365CustomerId,
                office365User.Office365UserId
            });
        }

        private async Task SendTransitionUserAndLicensesCommand(
            ExecuteContext<ITransitionDispatchCreatingUsersArguments> context, Office365UserModel office365User,
            UserModel user, RoleModel userRole, Office365TransitionProductItemModel item, bool keepLicences)
        {
            var arguments = context.Arguments;

            var isNewLicenses = !(item.Admin || item.RemoveLicenses);

            var userAndLicenseEndpoint = Office365ServiceConstants.QueueOffice365TransitionUserAndLicenseUri;
            var sendEndpoint = await context.GetSendEndpoint(userAndLicenseEndpoint);

            await sendEndpoint.Send<IOffice365TransitionUserAndLicensesCommand>(new
            {
                arguments.CompanyId,
                arguments.Office365CustomerId,
                CloudPlusUserExist = user != null,
                office365User.UserPrincipalName,
                office365User.Office365UserId,
                office365User.FirstName,
                office365User.LastName,
                office365User.DisplayName,
                KeepLicences = keepLicences,
                UserRoles = new List<string> { "Company Administrator" },
                Roles = new List<int> { userRole.Id },
                item.Password,
                item.RecommendedProductItem.CloudPlusProductIdentifier,
                IsNewLicenses = isNewLicenses,
                item.Admin,

            });
        }
    }
}
