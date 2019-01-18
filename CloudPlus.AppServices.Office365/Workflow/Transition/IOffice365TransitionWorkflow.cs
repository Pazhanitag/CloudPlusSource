using CloudPlus.QueueModels.Office365.Transition.Commands;
using CloudPlus.Workflows.Common.Workflow;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Workflow.Transition
{
    public interface IOffice365TransitionWorkflow : IWorkflow<ConsumeContext<IOffice365TransitionCommand>>
    {
    }
}
