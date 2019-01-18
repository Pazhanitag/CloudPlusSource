using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;

namespace CloudPlus.Workflows.Office365.Activities.Domain.GetCustomerTxtRecords
{
    public class GetCustomerDomainTxtRecordsActivity : IGetCustomerDomainTxtRecordsActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public GetCustomerDomainTxtRecordsActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IGetCustomerDomainTxtRecordsArguments> context)
        {
            var arguments = context.Arguments;

            var txtRecords =  await _office365ApiService.GetCustomerDomainTxtRecordsAsync(new Office365CustomerDomainModel
            {
                Domain = arguments.Domain,
                Office365CustomerId = arguments.Office365CustomerId
            });

            return context.CompletedWithVariables(new
            {
                TxtRecords = txtRecords
            });
        }
    }
}
