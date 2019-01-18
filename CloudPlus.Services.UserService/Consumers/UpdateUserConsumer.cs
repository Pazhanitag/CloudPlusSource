using System;
using System.Threading.Tasks;
using CloudPlus.AppServices.User.Workflow.UpdateUser;
using CloudPlus.QueueModels.Users.Commands;
using MassTransit;

namespace CloudPlus.AppServices.User.Consumers
{
    public class UpdateUserConsumer : IUpdateUserConsumer
    {
        private readonly IUpdateUserWorkflow _updateUserWorkflowBuilder;

        public UpdateUserConsumer(IUpdateUserWorkflow updateUserWorkflowBuilder)
        {
            _updateUserWorkflowBuilder = updateUserWorkflowBuilder;
        }
        public async Task Consume(ConsumeContext<IUpdateUserCommand> context)
        {
            await _updateUserWorkflowBuilder.Execute(context);
        }
    }
}
