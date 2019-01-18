using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Models.Office365.User;
using CloudPlus.Resources;
using CloudPlus.Services.Office365.CustomerService;
using CloudPlus.Services.Office365.User;
using MassTransit;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.CreateTempPartnerPlatformAdminUser
{

    public class CreateTempPartnerPlatformAdminUserActivity : ICreateTempPartnerPlatformAdminUserActivity
    {
        private readonly IOffice365UserService _office365UserService;
        private readonly IOffice365CustomerService _office365CustomerService;
        private readonly IConfigurationManager _configurationManager;

        public CreateTempPartnerPlatformAdminUserActivity(
            IOffice365UserService office365UserService, 
            IOffice365CustomerService office365CustomerService, 
            IConfigurationManager configurationManager)
        {
            _office365UserService = office365UserService;
            _office365CustomerService = office365CustomerService;
            _configurationManager = configurationManager;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateTempPartnerPlatformAdminUserArguments> context)
        {
            var args = context.Arguments;

            var customerDefaultMicrosoftDomain = await _office365CustomerService.GetCustomerDefaultDomainAsync(args.Office365CustomerId);

            var tempAdminUser = new Office365SdkUser
            {
                Office365CustomerId = args.Office365CustomerId,
                DisplayName = NewId.NextGuid().ToString("N"),
                FirstName = "TempAdmin",
                LastName = "TempAdmin",
                Password = _configurationManager.GetByKey("Office365TempPassword"),
                UsageLocation = "US",
                UserPrincipalName = $"{NewId.NextGuid():N}@{customerDefaultMicrosoftDomain}"
            };

            await _office365UserService.CreateOffice365UserAsync(tempAdminUser);

            return context.CompletedWithVariables(new
            {
                AdminUserName = tempAdminUser.UserPrincipalName,
                AdminPassword = tempAdminUser.Password,
                tempAdminUser.UserPrincipalName,
                UserRoles = new List<string>
                {
                    "Company Administrator"
                }
            });
        }
    }
}