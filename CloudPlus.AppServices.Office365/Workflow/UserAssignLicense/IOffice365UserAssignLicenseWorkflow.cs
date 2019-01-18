using MassTransit;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Workflows.Common.Workflow;

namespace CloudPlus.AppServices.Office365.Workflow.UserAssignLicense
{
    public interface IOffice365UserAssignLicenseWorkflow : IWorkflow<ConsumeContext<IOffice365UserAssignLicenseCommand>>
    {
    }
}
