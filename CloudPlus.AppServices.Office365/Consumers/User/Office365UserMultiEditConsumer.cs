using System;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.AppServices.Office365.Workflow;
using MassTransit;
using CloudPlus.Constants;
using CloudPlus.Models.Identity;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Identity.User;
using CloudPlus.Enums.Office365;
using CloudPlus.Models.Office365.User;
using CloudPlus.Services.Database.Office365.Subscription;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public class Office365UserMultiEditConsumer : IOffice365UserMultiEditConsumer
    {
        private readonly IUserService _userService;
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;
        private readonly IMultiUserEditWorkflow _multiUserEditWorkflow;
        public Office365UserMultiEditConsumer(
            IUserService userService,
            IOffice365DbCustomerService office365DbCustomerService,
            IOffice365DbSubscriptionService office365DbSubscriptionService, IMultiUserEditWorkflow multiUserEditWorkflow)
        {
            _userService = userService;
            _office365DbCustomerService = office365DbCustomerService;
            _office365DbSubscriptionService = office365DbSubscriptionService;
            _multiUserEditWorkflow = multiUserEditWorkflow;
        }

        public async Task Consume(ConsumeContext<IOffice365UserMultiEditCommand> context)
        {
            
            var messages = context.Message;
            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerAsync(messages.CompanyId);

            var subscription = await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                    office365Customer.Office365CustomerId, messages.CloudPlusProductIdentifier);
            
            if (subscription == null || subscription.SubscriptionState != Office365SubscriptionState.OperationInProgress)
            {
                await _multiUserEditWorkflow.Execute(context);
            }
            else
            {
                context.Headers.TryGetHeader("MT-Redelivery-Count", out var redeliverCount);
                if (redeliverCount == null || Convert.ToInt32(redeliverCount) < 100) // ????
                {
                    context.Redeliver(TimeSpan.FromSeconds(15));
                }
            }
        }

        private async void SendUserAssignLicense(ConsumeContext<IOffice365UserMultiEditCommand> context, UserModel user)
        {
            var messages = context.Message;

            var assignLicenseEndpoint = await context.GetSendEndpoint(Office365ServiceConstants.QueueOffice365UserAssignLicenseUri);

            await assignLicenseEndpoint.Send<IOffice365UserAssignLicenseCommand>(new
            {
                messages.CompanyId,
                messages.CloudPlusProductIdentifier,
                UserPrincipalName = user.UserName,
                user.DisplayName,
                user.FirstName,
                user.LastName,
                UsageLocation = "US",
                user.City,
                user.Country,
                user.PhoneNumber,
                PostalCode = user.ZipCode,
                user.State,
                user.StreetAddress,
                messages.UserRoles
            });
        }

        private async void SendUserChangeLicense(ConsumeContext<IOffice365UserMultiEditCommand> context, Office365UserModel office365User)
        {
            var messages = context.Message;

            var assignLicenseEndpoint = await context.GetSendEndpoint(Office365ServiceConstants.Office365UserChangeLicenseUri);

            await assignLicenseEndpoint.Send<IOffice365UserChangeLicenseCommand>(new
            {
                messages.CompanyId,
                office365User.UserPrincipalName,
                RemoveCloudPlusProductIdentifier = office365User.Licenses.FirstOrDefault()?.Office365Offer.CloudPlusProductIdentifier,
                AssignCloudPlusProductIdentifier = messages.CloudPlusProductIdentifier,
                messages.UserRoles
            });
        }

        private async void SendUseChangeRoles(ConsumeContext<IOffice365UserMultiEditCommand> context, UserModel user)
        {
            var messages = context.Message;

            var assignLicenseEndpoint = await context.GetSendEndpoint(Office365ServiceConstants.QueueOffice365ChangeUserRolesUri);

            await assignLicenseEndpoint.Send<IOffice365UserChangeRolesCommand>(new
            {
                messages.CompanyId,
                UserPrincipalName = user.UserName,
                messages.UserRoles
            });
        }
    }
}

