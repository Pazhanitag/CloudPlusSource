using MassTransit;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Workflows.Common.Workflow;

namespace CloudPlus.AppServices.Office365.Workflow.UserRestore
{
    public interface IOffice365UserRestoreWorkflow : IWorkflow<ConsumeContext<IOffice365UserRestoreCommand>>
    {
    }
}
