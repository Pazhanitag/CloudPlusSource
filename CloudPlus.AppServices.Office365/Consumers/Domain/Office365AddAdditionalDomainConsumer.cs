using System;
using System.Threading.Tasks;
using CloudPlus.AppServices.Office365.Workflow.AddAdditionalDomain;
using MassTransit;
using CloudPlus.QueueModels.Office365.Domain.Commands;

namespace CloudPlus.AppServices.Office365.Consumers.Domain
{
    public class Office365AddAdditionalDomainConsumer : IOffice365AddAdditionalDomainConsumer
    {
        private readonly IOffice365AddAdditionalDomainWorkflow _offceOffice365AddAdditionalDomainWorkflow;

        public Office365AddAdditionalDomainConsumer(IOffice365AddAdditionalDomainWorkflow offceOffice365AddAdditionalDomainWorkflow)
        {
            _offceOffice365AddAdditionalDomainWorkflow = offceOffice365AddAdditionalDomainWorkflow;
        }
        
        public async Task Consume(ConsumeContext<IOffice365AddAdditionalDomainCommand> context)
        {
            await _offceOffice365AddAdditionalDomainWorkflow.Execute(context);
        }
    }
}
