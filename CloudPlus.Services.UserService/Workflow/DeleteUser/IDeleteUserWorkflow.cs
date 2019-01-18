using CloudPlus.QueueModels.Users.Commands;
using CloudPlus.Workflows.Common.Workflow;
using MassTransit;

namespace CloudPlus.AppServices.User.Workflow.DeleteUser
{
	public interface IDeleteUserWorkflow : IWorkflow<ConsumeContext<IDeleteUserCommand>>
	{
		
	}
}