using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Database.Office365.User;
using CloudPlus.Services.Office365.User;

namespace CloudPlus.Workflows.Office365.Activities.User.RestorePartnerPlatformUser
{
    public class RestorePartnerPlatformUserActivity : IRestorePartnerPlatformUserActivity
    {
        private readonly IOffice365DbUserService _office365DbUserService;
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice365UserService _office365UserService;

        public RestorePartnerPlatformUserActivity(
            IOffice365DbUserService office365DbUserService,
            IOffice365DbCustomerService office365DbCustomerService,
            IOffice365UserService office365UserService)
        {
            _office365DbUserService = office365DbUserService;
            _office365DbCustomerService = office365DbCustomerService;
            _office365UserService = office365UserService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IRestorePartnerPlatformUserArguments> context)
        {
            var arguments = context.Arguments;

            var office365User = await _office365DbUserService.GetOffice365DatabaseUserAsync(arguments.UserPrincipalName);
            if (office365User == null)
                throw new Exception($"No Office365 user with upn {arguments.UserPrincipalName}");
            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerWithIncludesAsync(arguments.CompanyId);

            await _office365UserService.RestoreOffice365UserAsync(office365User.Office365UserId, office365Customer.Office365CustomerId);

            return context.Completed(new RestorePartnerPlatformUserLog
            {
                Office365UserId = office365User.Office365UserId,
                Office365CustomerId = office365Customer.Office365CustomerId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IRestorePartnerPlatformUserLog> context)
        {
            try
            {
                var logs = context.Log;

                await _office365UserService.DeleteOffice365UserAsync(logs.Office365UserId, logs.Office365CustomerId);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensate RestorePartnerPlatformUserActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
