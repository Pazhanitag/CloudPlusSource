using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;

namespace CloudPlus.Workflows.Office365.Activities.Domain.AddCustomerDomainPartnerPortalActivity
{
    public class AddCustomerDomainPartnerPortalActivity : IAddCustomerDomainPartnerPortalActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public AddCustomerDomainPartnerPortalActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IAddCustomerDomainPartnerPortalArguments> context)
        {
            var arguments = context.Arguments;

            await _office365ApiService.AddCustomerDomainAsync(new Office365CustomerDomainModel
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Domain = arguments.Domain
            });

            return context.Completed(new AddCustomerDomainPartnerPortalLog
            {
                Domain = arguments.Domain,
                Office365CustomerId = arguments.Office365CustomerId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IAddCustomerDomainPartnerPortalLog> context)
        {
            var log = context.Log;
            try
            {
                await _office365ApiService.RemoveCustomerDomainAsync(new Office365CustomerDomainModel
                {
                    Domain = log.Domain,
                    Office365CustomerId = log.Office365CustomerId
                });
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Could not compensate for AddCustomerDomainPartnerPortalActivity.", ex);
            }
            return context.Compensated();
        }
    }
}
