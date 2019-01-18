using System;
using System.Threading.Tasks;
using CloudPlus.AppServices.User.Workflow.CreateUser;
using CloudPlus.QueueModels.Users.Commands;
using CloudPlus.Services.Database.WorkflowActivity;
using CloudPlus.Services.Identity.User;
using MassTransit;

namespace CloudPlus.AppServices.User.Consumers
{
    public class CreateUserConsumer : ICreateUserConsumer
    {
        private readonly ICreateUserWorkflow _createUserWorkflowBuilder;
        private readonly IWorkflowUserActivityService _workflowUserActivityService;
        private readonly IUserService _userService;
        public CreateUserConsumer(
            ICreateUserWorkflow createUserWorkflowBuilder, 
            IWorkflowUserActivityService workflowUserActivityService, 
            IUserService userService)
        {
            _createUserWorkflowBuilder = createUserWorkflowBuilder;
            _workflowUserActivityService = workflowUserActivityService;
            _userService = userService;
        }
        public async Task Consume(ConsumeContext<ICreateUserCommand> context)
        {
            if (string.IsNullOrWhiteSpace(context.Message.Email))
                throw new ArgumentException(nameof(context.Message.Email));

            var existingUser = await _userService.GetUserAsync(context.Message.Email);

            if (existingUser != null)
                throw new Exception("User already exists");

            if (_workflowUserActivityService.IsUserBeingCreated(context.Message.Email))
                throw new Exception("Create user already started");

            await _createUserWorkflowBuilder.Execute(context);
        }
    }
}