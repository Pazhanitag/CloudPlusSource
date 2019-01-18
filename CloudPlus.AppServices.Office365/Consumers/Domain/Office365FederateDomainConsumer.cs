using System.Threading.Tasks;
using CloudPlus.AppServices.Office365.Workflow.FederateDomain;
using CloudPlus.QueueModels.Office365.Domain.Federate;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Consumers.Domain
{
    public interface IOffice365FederateDomainConsumer : IConsumer<IOffice365FederateDomainRequest>
    {

    }
    public class Office365FederateDomainConsumer : IOffice365FederateDomainConsumer
    {
        private readonly IFederateDomainWorkflow _federateDomainWorkflow;

        public Office365FederateDomainConsumer(IFederateDomainWorkflow federateDomainWorkflow)
        {
            _federateDomainWorkflow = federateDomainWorkflow;
        }
        public async Task Consume(ConsumeContext<IOffice365FederateDomainRequest> context)
        {
            await _federateDomainWorkflow.Execute(context);
        }
    }
}