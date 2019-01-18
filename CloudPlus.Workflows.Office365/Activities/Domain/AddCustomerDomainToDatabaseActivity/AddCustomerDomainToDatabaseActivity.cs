using System.Threading.Tasks;
using CloudPlus.Enums.Office365;
using MassTransit.Courier;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Domain;

namespace CloudPlus.Workflows.Office365.Activities.Domain.AddCustomerDomainToDatabaseActivity
{
    public class AddCustomerDomainToDatabaseActivity : IAddCustomerDomainToDatabaseActivity
    {
        private readonly IOffice365DbDomainService _office365DbDomainService;

        public AddCustomerDomainToDatabaseActivity(IOffice365DbDomainService office365DbDomainService)
        {
            _office365DbDomainService = office365DbDomainService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IAddCustomerDomainToDatabaseArguments> context)
        {
            var arguments = context.Arguments;

            await _office365DbDomainService.CreateDatabaseCustomerDomainAsync(new Office365CustomerDomainModel
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Domain = arguments.Domain,
                Office365DomainStaus = Office365DomainStatus.NotValidated
            });

            return context.Completed();
        }
    }
}
