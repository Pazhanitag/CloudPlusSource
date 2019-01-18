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
using CloudPlus.Workflows.Office365.Activities.User.ActivateSoftDeletedDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.RestorePartnerPlatformUser;
using CloudPlus.Workflows.Office365.Mappers;

namespace CloudPlus.AppServices.Office365.Workflow.UserRestore
{
    public class Office365UserRestoreWorkflow : IOffice365UserRestoreWorkflow
    {
        private readonly IActivityOffice365UserRestoreMapper _mapper;
        private readonly IActivityConfigurator _activityConfigurator;

        public Office365UserRestoreWorkflow(IActivityOffice365UserRestoreMapper mapper, IActivityConfigurator activityConfigurator)
        {
            _mapper = mapper;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<IOffice365UserRestoreCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var command = context.Message;

            builder.AddActivity(Office365ServiceConstants.ActivityRestorePartnerPlatformUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IRestorePartnerPlatformUserActivity)),
                _mapper.MapRestorePartnerPlatformUserArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityActivateSoftDeletedDatabaseUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IActivateSoftDeletedDatabaseUserActivity)),
                _mapper.MapActivateSoftDeletedDatabaseUserArguments(command));

            builder.AddVariable("userPrincipalName", context.Message.UserPrincipalName);
            builder.AddVariable("workflowActivityType", WorkflowActivityType.Office365UserRestore.ToString());

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
                WorkflowActivityType = WorkflowActivityType.Office365UserRestore.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}
