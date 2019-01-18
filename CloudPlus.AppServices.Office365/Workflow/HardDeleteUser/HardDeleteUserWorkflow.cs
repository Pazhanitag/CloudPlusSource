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
using CloudPlus.Workflows.Office365.Activities.User.DeleteDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.HardDeletePartnerPlatformUser;

namespace CloudPlus.AppServices.Office365.Workflow.HardDeleteUser
{
    public class HardDeleteUserWorkflow : IHardDeleteUserWorkflow
    {
        private readonly IActivityConfigurator _activityConfigurator;

        public HardDeleteUserWorkflow(IActivityConfigurator activityConfigurator)
        {
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<IOffice365HardDeleteUserCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var command = context.Message;

            builder.AddActivity(Office365ServiceConstants.ActivityHardDeleteDatabaseUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IDeleteDatabaseUserActivity)),
                new
                {
                    command.UserPrincipalName,
                    WorkflowActivityType = WorkflowActivityType.Office365HardDeleteUser,
                    WorkflowStep = WorkflowActivityStep.Office365HardDeleteDatabaseUser
                });

            builder.AddActivity(Office365ServiceConstants.ActivityHardDeletePartnerPlatformUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IHardDeletePartnerPlatformUserActivity)),
                new
                {
                    command.Office365CustomerId,
                    command.UserPrincipalName,
                    WorkflowActivityType = WorkflowActivityType.Office365HardDeleteUser,
                    WorkflowStep = WorkflowActivityStep.Office365HardDeletePartnerPortalUser
                });

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
                WorkflowActivityType = WorkflowActivityType.Office365HardDeleteUser.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}
