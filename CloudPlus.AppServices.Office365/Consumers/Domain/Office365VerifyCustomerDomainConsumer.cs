using System.Threading.Tasks;
using CloudPlus.AppServices.Office365.Workflow.VerifyDomain;
using MassTransit;
using CloudPlus.QueueModels.Office365.Domain.Commands;

namespace CloudPlus.AppServices.Office365.Consumers.Domain
{
    public class Office365VerifyCustomerDomainConsumer : IOffice365VerifyCustomerDomainConsumer
    {
        private readonly IOffice365VerifyDomainWorkflow _office365VerifyDomainWorkflow;

        public Office365VerifyCustomerDomainConsumer(IOffice365VerifyDomainWorkflow office365VerifyDomainWorkflow)
        {
            _office365VerifyDomainWorkflow = office365VerifyDomainWorkflow;
        }

        public async Task Consume(ConsumeContext<IOffice365VerifyDomainCommand> context)
        {
            await _office365VerifyDomainWorkflow.Execute(context);
        }
    }
}
