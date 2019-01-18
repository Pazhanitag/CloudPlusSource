using MassTransit;
using CloudPlus.QueueModels.Office365.Transition.Commands;
using CloudPlus.Workflows.Common.Workflow;

namespace CloudPlus.AppServices.Office365.Workflow.Transition
{
    public interface IOffice365TransitionUserAndLicensesWorkflow
        : IWorkflow<ConsumeContext<IOffice365TransitionUserAndLicensesCommand>>
    {
    }
}
