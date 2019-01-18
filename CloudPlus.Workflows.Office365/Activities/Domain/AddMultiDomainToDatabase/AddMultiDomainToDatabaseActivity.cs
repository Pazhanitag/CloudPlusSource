using System;
using System.Threading.Tasks;
using CloudPlus.Enums.Office365;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Domain;

namespace CloudPlus.Workflows.Office365.Activities.Domain.AddMultiDomainToDatabase
{
    public class AddMultiDomainToDatabaseActivity : IAddMultiDomainToDatabaseActivity
    {
        private readonly IOffice365DbDomainService _office365DbDomainService;

        public AddMultiDomainToDatabaseActivity(IOffice365DbDomainService office365DbDomainService)
        {
            _office365DbDomainService = office365DbDomainService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IAddMultiDomainToDatabaseArguments> context)
        {
            var arguments = context.Arguments;

            foreach (var domain in arguments.Domains)
            {
                await _office365DbDomainService.CreateDatabaseCustomerDomainAsync(new Office365CustomerDomainModel
                {
                    Office365CustomerId = arguments.Office365CustomerId,
                    Domain = domain,
                    Office365DomainStaus = Office365DomainStatus.Validated
                });
            }

            return context.Completed(new AddMultiDomainToDatabaseLog
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Domains = arguments.Domains
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IAddMultiDomainToDatabaseLog> context)
        {
            try
            {
                var logs = context.Log;

                foreach (var domain in logs.Domains)
                {
                    await _office365DbDomainService.DeleteDomainAsync(domain);
                }
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensate AddMultiDomainToDatabaseActivity fail!", ex);
            }

            return context.Compensated();
        }
    }
}
