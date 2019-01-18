using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Office365.User;

namespace CloudPlus.Workflows.Office365.Activities.User.CreatePartnerPlatformUser
{
    public class CreatePartnerPlatformUserActivity : ICreatePartnerPlatformUserActivity
    {
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice365ApiService _office365ApiService;
        private readonly IOffice365UserService _office365UserService;
        public CreatePartnerPlatformUserActivity(
            IOffice365ApiService office365ApiService,
            IOffice365DbCustomerService office365DbCustomerService, IOffice365UserService office365UserService)
        {
            _office365ApiService = office365ApiService;
            _office365DbCustomerService = office365DbCustomerService;
            _office365UserService = office365UserService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ICreatePartnerPlatformUserArguments> context)
        {
            var arguments = context.Arguments;

            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerWithIncludesAsync(arguments.CompanyId);

            if (office365Customer == null)
                throw new ArgumentException($"No Office 365 Customer in database with Comapny Id: {arguments.CompanyId}");

            string office365UserId = null;

            var existingOffice365UserId = await _office365UserService.GetOffice365UserIdAsync(arguments.UserPrincipalName,
                office365Customer.Office365CustomerId);

            if (existingOffice365UserId == null)
            {
                office365UserId = await _office365ApiService.CreateOffice365UserAsync(new Office365ApiUserModel
                {
                    Office365CustomerId = office365Customer.Office365CustomerId,
                    UserPrincipalName = arguments.UserPrincipalName,
                    DisplayName = arguments.DisplayName,
                    FirstName = arguments.FirstName,
                    LastName = arguments.LastName,
                    UsageLocation = arguments.UsageLocation,
                    City = arguments.City,
                    Country = arguments.Country,
                    PhoneNumber = arguments.PhoneNumber,
                    PostalCode = arguments.PostalCode,
                    State = arguments.State,
                    StreetAddress = arguments.StreetAddress,
                    Password = arguments.Password
                });
            }
            return context.CompletedWithVariables(new CreatePartnerPlatformUserLog
            {
                Office365CustomerId = office365Customer.Office365CustomerId,
                UserPrincipalName = arguments.UserPrincipalName
            }, new
            {
                office365Customer.Office365CustomerId,
                Office365UserId = office365UserId ?? existingOffice365UserId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ICreatePartnerPlatformUserLog> context)
        {
            try
            {
                var log = context.Log;

                await _office365ApiService.UserHardDeleteAsync(new Office365ApiUserModel
                {
                    Office365CustomerId = log.Office365CustomerId,
                    UserPrincipalName = log.UserPrincipalName
                });
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating CreatePartnerPlatformUserActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
