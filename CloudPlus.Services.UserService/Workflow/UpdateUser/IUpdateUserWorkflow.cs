using CloudPlus.QueueModels.Users.Commands;
using CloudPlus.Workflows.Common.Workflow;
using MassTransit;

namespace CloudPlus.AppServices.User.Workflow.UpdateUser
{
    public interface IUpdateUserWorkflow : IWorkflow<ConsumeContext<IUpdateUserCommand>>
    {
    }
}