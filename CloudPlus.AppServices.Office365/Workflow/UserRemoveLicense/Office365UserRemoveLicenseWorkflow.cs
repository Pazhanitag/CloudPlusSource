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
using CloudPlus.Workflows.Office365.Activities.User.DeletePartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.SoftDeleteDatabaseUser;
using CloudPlus.Workflows.Office365.Mappers;

namespace CloudPlus.AppServices.Office365.Workflow.UserRemoveLicense
{
    public class Office365UserRemoveLicenseWorkflow : IOffice365UserRemoveLicenseWorkflow
    {
        private readonly IActivityOffice365UserArgumentsMapper _mapper;
        private readonly IActivityConfigurator _activityConfigurator;

        public Office365UserRemoveLicenseWorkflow(IActivityOffice365UserArgumentsMapper mapper, IActivityConfigurator activityConfigurator)
        {
            _mapper = mapper;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<IOffice365UserRemoveLicenseCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var command = context.Message;

            builder.AddActivity(Office365ServiceConstants.ActivityDeletePartnerPlatformUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IDeletePartnerPlatformUserActivity)), 
                _mapper.MapDeletePartnerPlatformUserArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivitySoftDeleteDatabaseUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ISoftDeleteDatabaseUserActivity)), 
                _mapper.MapSoftDeleteDatabaseUserArguments(command));

            builder.AddVariable("userPrincipalName", context.Message.UserPrincipalName);
            builder.AddVariable("workflowActivityType", WorkflowActivityType.Office365UserRemoveLicense.ToString());

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
                WorkflowActivityType = WorkflowActivityType.Office365UserRemoveLicense.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}
