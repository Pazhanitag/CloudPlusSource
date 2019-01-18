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
using CloudPlus.Workflows.User.Activities.UpdateActiveDirectoryUser;
using CloudPlus.Workflows.User.Activities.UpdateIdentityServerUser;
using CloudPlus.Workflows.User.Mappers;

namespace CloudPlus.AppServices.User.Workflow.UpdateUser
{
    public class UpdateUserWorkflow : IUpdateUserWorkflow
    {
        private readonly IActivityUserArgumentsMapper _activityArgumentsMapper;
        private readonly IActivityConfigurator _activityConfigurator;

        public UpdateUserWorkflow(IActivityUserArgumentsMapper activityArgumentsMapper, IActivityConfigurator activityConfigurator)
        {
            _activityArgumentsMapper = activityArgumentsMapper;
            _activityConfigurator = activityConfigurator;
        }
        public async Task Execute(ConsumeContext<IUpdateUserCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            var updateUserCommand = context.Message;

            builder.AddActivity(UserServiceConstants.ActivityUpdateAdUser, _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateActiveDirectoryUserActivity)), _activityArgumentsMapper.MapActiveDirectoryUserArguments(updateUserCommand));

            builder.AddActivity(UserServiceConstants.ActivityUpdateIsUser, _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateIdentityServerUserActivity)), _activityArgumentsMapper.MapIdentityServerUserArguments(updateUserCommand));

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
                WorkflowActivityType = WorkflowActivityType.UpdateUser.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}
