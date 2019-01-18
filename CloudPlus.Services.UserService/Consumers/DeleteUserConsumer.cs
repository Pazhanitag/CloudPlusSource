using System.Threading.Tasks;
using CloudPlus.AppServices.User.Workflow.DeleteUser;
using CloudPlus.QueueModels.Users.Commands;
using MassTransit;

namespace CloudPlus.AppServices.User.Consumers
{
	public class DeleteUserConsumer : IDeleteUserConsumer
	{
		private readonly IDeleteUserWorkflow _deleteUserWorkflowBuilder;

		public DeleteUserConsumer(IDeleteUserWorkflow deleteUserWorkflow)
		{
			_deleteUserWorkflowBuilder = deleteUserWorkflow;
		}

		public async Task Consume(ConsumeContext<IDeleteUserCommand> context)
		{
			await _deleteUserWorkflowBuilder.Execute(context);
		}
	}
}