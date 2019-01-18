using CloudPlus.QueueModels.Users.Commands;
using CloudPlus.Workflows.Common.Workflow;
using MassTransit;

namespace CloudPlus.AppServices.User.Workflow.CreateUser
{
    public interface ICreateUserWorkflow : IWorkflow<ConsumeContext<ICreateUserCommand>>
    {
    }
}