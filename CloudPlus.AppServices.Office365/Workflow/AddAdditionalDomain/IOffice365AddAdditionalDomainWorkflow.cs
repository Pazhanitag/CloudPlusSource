using MassTransit;
using CloudPlus.QueueModels.Office365.Domain.Commands;
using CloudPlus.Workflows.Common.Workflow;

namespace CloudPlus.AppServices.Office365.Workflow.AddAdditionalDomain
{
    public interface IOffice365AddAdditionalDomainWorkflow : IWorkflow<ConsumeContext<IOffice365AddAdditionalDomainCommand>>
    {
    }
}
