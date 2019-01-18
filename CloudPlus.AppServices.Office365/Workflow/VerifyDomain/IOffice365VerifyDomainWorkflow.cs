using CloudPlus.QueueModels.Office365.Domain;
using CloudPlus.QueueModels.Office365.Domain.Commands;
using CloudPlus.Workflows.Common.Workflow;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Workflow.VerifyDomain
{
    public interface IOffice365VerifyDomainWorkflow : IWorkflow<ConsumeContext<IOffice365VerifyDomainCommand>>
    {

    }
}