using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Database.Office365.User;
using CloudPlus.Services.Office365.User;

namespace CloudPlus.Workflows.Office365.Activities.User.DeletePartnerPlatformUser
{
    public class DeletePartnerPlatformUserActivity : IDeletePartnerPlatformUserActivity
    {
        private readonly IOffice365UserService _office365UserService;
        private readonly IOffice365DbUserService _office365DbUserService;
        private readonly IOffice365DbCustomerService _office365DbCustomerService;

        public DeletePartnerPlatformUserActivity(
            IOffice365UserService office365UserService,
            IOffice365DbUserService office365DbUserService,
            IOffice365DbCustomerService office365DbCustomerService)
        {
            _office365UserService = office365UserService;
            _office365DbUserService = office365DbUserService;
            _office365DbCustomerService = office365DbCustomerService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IDeletePartnerPlatformUserArguments> context)
        {
            var arguments = context.Arguments;

            var office365User = await _office365DbUserService.GetOffice365DatabaseUserWithLicensesAndOfferAsync(arguments.UserPrincipalName);
            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerWithIncludesAsync(arguments.CompanyId);

            if (office365User == null)
                throw new ArgumentException($"No Office 365 User with User Principal Name: {arguments.UserPrincipalName}");

            if (office365Customer == null)
                throw new ArgumentException($"No Office 365 Customer with Comapny Id: {arguments.CompanyId}");

            await _office365UserService.DeleteOffice365UserAsync(office365User.Office365UserId, office365Customer.Office365CustomerId);

            return context.CompletedWithVariables(new DeletePartnerPlatformUserLog
            {
                Office365UserId = office365User.Office365UserId,
                Office365CustomerId = office365Customer.Office365CustomerId
            }, new
            {
                office365User.Licenses.FirstOrDefault()?.Office365Offer.CloudPlusProductIdentifier
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IDeletePartnerPlatformUserLog> context)
        {
            try
            {
                var logs = context.Log;

                await _office365UserService.RestoreOffice365UserAsync(logs.Office365UserId, logs.Office365CustomerId);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating DeletePartnerPlatformUserActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
