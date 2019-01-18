using CloudPlus.QueueModels.Office365.Domain.Federate;
using CloudPlus.Workflows.Common.Workflow;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Workflow.FederateDomain
{
    public interface IFederateDomainWorkflow : IWorkflow<ConsumeContext<IOffice365FederateDomainRequest>>
    {

    }
}