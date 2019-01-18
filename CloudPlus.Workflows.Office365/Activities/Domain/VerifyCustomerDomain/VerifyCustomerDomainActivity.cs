using System.Threading.Tasks;
using CloudPlus.Exceptions.Office365;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Office365.Domain;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Domain.VerifyCustomerDomain
{
    public class VerifyCustomerDomainActivity : IVerifyCustomerDomainActivity
    {
        private readonly IOffice365DomainService _office365DomainService;

        public VerifyCustomerDomainActivity(IOffice365DomainService office365DomainService)
        {
            _office365DomainService = office365DomainService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IVerifyCustomerDomainArguments> context)
        {
            var arguments = context.Arguments;

            var domainVerified = await _office365DomainService.IsDomainVerified(new Office365CustomerDomainModel
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Domain = arguments.DomainName
            });

            if (domainVerified)
                return context.Completed();

            var verifyDomain = await _office365DomainService.VerifyCustomerDomain(new Office365CustomerDomainModel
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Domain = arguments.DomainName
            });

            if (!verifyDomain)
                throw new Office365VerifyDomainException(
                    $"Could not verify domain {arguments.DomainName} for office 365 customer id {arguments.Office365CustomerId}");

            return context.Completed();
        }
    }
}