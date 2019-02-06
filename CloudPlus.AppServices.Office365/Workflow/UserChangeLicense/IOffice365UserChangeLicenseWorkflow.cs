using MassTransit;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Workflows.Common.Workflow;

namespace CloudPlus.AppServices.Office365.Workflow.UserChangeLicense
{
    public interface IOffice365UserChangeLicenseWorkflow : IWorkflow<ConsumeContext<IOffice365UserChangeLicenseCommand>>
    {
    }
}
