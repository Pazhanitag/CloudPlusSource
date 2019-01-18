using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using CloudPlus.Constants;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Common;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToPartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.AssignUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.GetUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.RemoveLicenseDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.RemoveLicensePartnerPortalUser;
using CloudPlus.Workflows.Office365.Activities.User.RemoveUserRoles;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public class Office365UserChangeLicenseConsumer : IOffice365UserChangeLicenseConsumer
    {
        private readonly IActivityConfigurator _activityConfigurator;

        public Office365UserChangeLicenseConsumer(IActivityConfigurator activityConfigurator)
        {
            _activityConfigurator = activityConfigurator;
        }

        public async Task Consume(ConsumeContext<IOffice365UserChangeLicenseCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var command = context.Message;

            builder.AddActivity(Office365ServiceConstants.ActivityRemoveLicensePartnerPortalUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IRemoveLicensePartnerPortalUserActivity)),
                new
                {
                    command.UserPrincipalName,
                    command.Office365CustomerId,
                    CloudPlusProductIdentifier = command.RemoveCloudPlusProductIdentifier,
                    WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                    WorkflowStep = WorkflowActivityStep.Office365RemoveLicensePartnerPortalUser
                });

            builder.AddActivity(Office365ServiceConstants.ActivityRemoveLicenseDatabaseUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IRemoveLicenseDatabaseUserActivity)),
                new
                {
                    command.UserPrincipalName,
                    command.Office365CustomerId,
                    CloudPlusProductIdentifier = command.RemoveCloudPlusProductIdentifier,
                    WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                    WorkflowStep = WorkflowActivityStep.Office365RemoveLicenseDatabaseUser
                });

            builder.AddActivity(Office365ServiceConstants.ActivityAssignLicenseOffice365PartnerPlatformUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignLicenseToPartnerPlatformUserActivity)),
                new
                {
                    command.UserPrincipalName,
                    command.Office365CustomerId,
                    CloudPlusProductIdentifier = command.AssignCloudPlusProductIdentifier,
                    WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                    WorkflowStep = WorkflowActivityStep.Office365AssignLicenseToPartnerPlatformUser
                });

            builder.AddActivity(Office365ServiceConstants.ActivityAssignLicenseOffice365DatabaseUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignLicenseToDatabaseUserActivity)),
                new
                {
                    command.UserPrincipalName,
                    command.Office365CustomerId,
                    CloudPlusProductIdentifier = command.AssignCloudPlusProductIdentifier,
                    WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                    WorkflowStep = WorkflowActivityStep.Office365AssignLicenseToDatabaseUser
                });

            builder.AddActivity(Office365ServiceConstants.ActivityGetUserRoles,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IGetUserRolesActivity)),
                new
                {
                    command.CompanyId,
                    command.UserPrincipalName,
                    command.Office365CustomerId,
                    WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                    WorkflowStep = WorkflowActivityStep.Office365GetUserRoles
                });

            builder.AddActivity(Office365ServiceConstants.ActivityRemoveUserRoles,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IRemoveUserRolesActivity)),
                new
                {
                    command.UserPrincipalName,
                    command.Office365CustomerId,
                    WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                    WorkflowStep = WorkflowActivityStep.Office365RemoveUserRoles
                });

            if (command.UserRoles.Any())
            {
                builder.AddActivity(Office365ServiceConstants.ActivityAssignUserRoles,
                    _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignUserRolesActivity)),
                    new
                    {
                        command.UserRoles,
                        command.Office365CustomerId,
                        command.UserPrincipalName,
                        WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense,
                        WorkflowStep = WorkflowActivityStep.Office365AssignUserRoles
                    });
            }

            builder.AddVariable("userPrincipalName", context.Message.UserPrincipalName);
            builder.AddVariable("workflowActivityType", WorkflowActivityType.Office365UserChangeLicense.ToString());

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
                WorkflowActivityType = WorkflowActivityType.Office365UserChangeLicense.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}
