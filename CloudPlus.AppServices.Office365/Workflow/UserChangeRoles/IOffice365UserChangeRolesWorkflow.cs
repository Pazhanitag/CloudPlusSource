using MassTransit;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Workflows.Common.Workflow;

namespace CloudPlus.AppServices.Office365.Workflow.UserChangeRoles
{
    public interface IOffice365UserChangeRolesWorkflow : IWorkflow<ConsumeContext<IOffice365UserChangeRolesCommand>>
    {
    }
}
