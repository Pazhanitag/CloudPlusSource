using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Services.Database.Office365.Domain;

namespace CloudPlus.Workflows.Office365.Activities.Domain.FederateCustomerDomainDatabaseStatus
{
    public class FederateCustomerDomainDatabaseStatusActivity : IFederateCustomerDomainDatabaseStatusActivity
    {
        private readonly IOffice365DbDomainService _office365DbDomainService;

        public FederateCustomerDomainDatabaseStatusActivity(IOffice365DbDomainService office365DbDomainService)
        {
            _office365DbDomainService = office365DbDomainService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IFederateCustomerDomainDatabaseStatusArguments> context)
        {
            var arguments = context.Arguments;

            await _office365DbDomainService.ChangeDatabaseCustomerDomainFederatedStatusAsync(arguments.DomainName, true);

            return context.Completed();
        }
    }
}
