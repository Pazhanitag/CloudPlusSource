using MassTransit;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Workflows.Common.Workflow;

namespace CloudPlus.AppServices.Office365.Workflow.UserRemoveLicense
{
    public interface IOffice365UserRemoveLicenseWorkflow : IWorkflow<ConsumeContext<IOffice365UserRemoveLicenseCommand>>
    {
    }
}
