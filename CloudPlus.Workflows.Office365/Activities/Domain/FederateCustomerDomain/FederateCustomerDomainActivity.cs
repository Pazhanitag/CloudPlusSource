using System.Threading.Tasks;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Domain.FederateCustomerDomain
{
    public class FederateCustomerDomainActivity : IFederateCustomerDomainActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public FederateCustomerDomainActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IFederateCustomerDomainArguments> context)
        {
            var arguments = context.Arguments;

            var domainFederated = await _office365ApiService.IsDomainfederated(new Office365CustomerDomainModel
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Domain = arguments.DomainName
            });

            if (domainFederated)
                return context.Completed();

            await _office365ApiService.FederateCustomerDomainAsync(new Office365CustomerDomainModelWithCredentials
            {
                AdminUserName = arguments.AdminUserName,
                AdminPassword = arguments.AdminPassword,
                Office365CustomerId = arguments.Office365CustomerId,
                Domain = arguments.DomainName
            });

            return context.Completed();
        }
    }
}