using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using CloudPlus.Constants;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Common;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Office365.Activities.User.AssignUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.GetUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.RemoveUserRoles;
using CloudPlus.Workflows.Office365.Mappers;

namespace CloudPlus.AppServices.Office365.Workflow.UserChangeRoles
{
    public class Office365UserChangeRolesWorkflow : IOffice365UserChangeRolesWorkflow
    {
        private readonly IActivityOffice365UserChangeRolesMapper _mapper;
        private readonly IActivityConfigurator _activityConfigurator;

        public Office365UserChangeRolesWorkflow(IActivityOffice365UserChangeRolesMapper mapper, IActivityConfigurator activityConfigurator)
        {
            _mapper = mapper;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<IOffice365UserChangeRolesCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var command = context.Message;

            builder.AddActivity(Office365ServiceConstants.ActivityGetUserRoles,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IGetUserRolesActivity)), 
                _mapper.MapGetUserRolesArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityRemoveUserRoles,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IRemoveUserRolesActivity)), 
                _mapper.MapRemoveUserRolesArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityAssignUserRoles,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignUserRolesActivity)), 
                _mapper.MapAssignUserRolesArguments(command));

            builder.AddVariable("userPrincipalName", context.Message.UserPrincipalName);
            builder.AddVariable("workflowActivityType", WorkflowActivityType.Office365UserChangeRoles.ToString());

            builder.AddSubscription(Office365ServiceConstants.QueueOffice365RoutingSlipEventUri,
                RoutingSlipEvents.Completed |
                RoutingSlipEvents.Faulted |
                RoutingSlipEvents.ActivityCompleted |
                RoutingSlipEvents.ActivityFaulted |
                RoutingSlipEvents.ActivityCompensated |
                RoutingSlipEvents.ActivityCompensationFailed);

            var routingSlip = builder.Build();

            await context.Send<IRoutingSlipStarted>(Office365ServiceConstants.QueueOffice365RoutingSlipStartedUri, new
            {
                builder.TrackingNumber,
                CreateTimestamp = DateTime.UtcNow,
                Arguments = context.Message,
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeRoles.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}
