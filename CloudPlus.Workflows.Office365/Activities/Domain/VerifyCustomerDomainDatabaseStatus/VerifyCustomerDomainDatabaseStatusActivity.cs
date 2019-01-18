using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Enums.Office365;
using CloudPlus.Services.Database.Office365.Domain;

namespace CloudPlus.Workflows.Office365.Activities.Domain.VerifyCustomerDomainDatabaseStatus
{
    public class VerifyCustomerDomainDatabaseStatusActivity : IVerifyCustomerDomainDatabaseStatusActivity
    {
        private readonly IOffice365DbDomainService _office365DbDomainService;

        public VerifyCustomerDomainDatabaseStatusActivity(IOffice365DbDomainService office365DbDomainService)
        {
            _office365DbDomainService = office365DbDomainService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IVerifyCustomerDomainDatabaseStatusArguments> context)
        {
            var arguments = context.Arguments;

            await _office365DbDomainService.ChangeDatabaseCustomerDomainVerifyStatusAsync(arguments.DomainName,
                Office365DomainStatus.Validated);

            return context.Completed();
        }
    }
}
