using CloudPlus.AppServices.Office365.Consumers.User;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Workflows.Common.Workflow;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Workflow.HardDeleteUser
{
    public interface IHardDeleteUserWorkflow : IWorkflow<ConsumeContext<IOffice365HardDeleteUserCommand>>
    {
        
    }
}