using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using CloudPlus.Constants;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Common;
using CloudPlus.QueueModels.Users.Commands;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.User.Activities.CreateActiveDirectoryUser;
using CloudPlus.Workflows.User.Activities.CreateIdentityServerUser;
using CloudPlus.Workflows.User.Activities.UserCreated;
using CloudPlus.Workflows.User.Mappers;

namespace CloudPlus.AppServices.User.Workflow.CreateUser
{
    public class CreateUserWorkflow : ICreateUserWorkflow
    {
        private readonly IActivityUserArgumentsMapper _activityArgumentsMapper;
        private readonly IActivityConfigurator _activityConfigurator;

        public CreateUserWorkflow(IActivityUserArgumentsMapper activityArgumentsMapper,
            IActivityConfigurator activityConfigurator)
        {
            _activityArgumentsMapper = activityArgumentsMapper;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<ICreateUserCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var createUserCommand = context.Message;

            builder.AddActivity(UserServiceConstants.ActivityCreateAdUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateActiveDirectoryUserActivity)),
                _activityArgumentsMapper.MapActiveDirectoryUserArguments(createUserCommand));
            
            builder.AddActivity(UserServiceConstants.ActivityCreateIsUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateIdentityServerUserActivity)),
                _activityArgumentsMapper.MapIdentityServerUserArguments(createUserCommand));
            
            builder.AddActivity(UserServiceConstants.ActivityUserCreated,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IUserCreatedActivity)),
                _activityArgumentsMapper.MapUserCreatedArguments(createUserCommand));

            builder.AddSubscription(UserServiceConstants.RoutingSlipEventObserverUri,
                RoutingSlipEvents.Completed |
                RoutingSlipEvents.Faulted |
                RoutingSlipEvents.ActivityCompleted |
                RoutingSlipEvents.ActivityFaulted |
                RoutingSlipEvents.ActivityCompensated |
                RoutingSlipEvents.ActivityCompensationFailed);

            var routingSlip = builder.Build();

            await context.Send<IRoutingSlipStarted>(UserServiceConstants.RoutingSlipUserStartedEventUri, new
            {
                builder.TrackingNumber,
                CreateTimestamp = DateTime.UtcNow,
                Arguments = context.Message,
                WorkflowActivityType = WorkflowActivityType.CreateUser.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}