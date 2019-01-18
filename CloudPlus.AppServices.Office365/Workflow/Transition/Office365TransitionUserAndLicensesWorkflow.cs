using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using CloudPlus.Constants;
using CloudPlus.Logging;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Common;
using CloudPlus.QueueModels.Office365.Transition.Commands;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToPartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.AssignUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.CreateDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.GetUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.RemoveAllLicensesPartnerPortalUser;
using CloudPlus.Workflows.Office365.Activities.User.RemoveUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.SetImmutableId;
using CloudPlus.Workflows.Office365.Mappers;
using CloudPlus.Workflows.User.Activities.CreateActiveDirectoryUser;
using CloudPlus.Workflows.User.Activities.CreateIdentityServerUser;

namespace CloudPlus.AppServices.Office365.Workflow.Transition
{
    public class Office365TransitionUserAndLicensesWorkflow : IOffice365TransitionUserAndLicensesWorkflow
    {
        private readonly IActivityOffice365TransitionUserAndLicensesArgumentsMapper _mapper;
        private readonly IActivityConfigurator _activityConfigurator;

        public Office365TransitionUserAndLicensesWorkflow(
            IActivityOffice365TransitionUserAndLicensesArgumentsMapper mapper,
            IActivityConfigurator activityConfigurator)
        {
            _mapper = mapper;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<IOffice365TransitionUserAndLicensesCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var command = context.Message;

            this.Log().Info($"01 - User: {command.UserPrincipalName}");
            this.Log().Info($"Admin: {command.Admin}");
            this.Log().Info($"RemoveLicenses: {command.RemoveLicenses}");
            this.Log().Info($"CloudPlusUserExist: {command.CloudPlusUserExist}");
            this.Log().Info($"IsNewLicenses: {command.Admin}");

            if (!command.CloudPlusUserExist)
            {
                this.Log().Info($"02 - Enter in CloudPlusExist: {command.UserPrincipalName}");

                builder.AddActivity(UserServiceConstants.ActivityCreateAdUser,
                    _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateActiveDirectoryUserActivity)),
                    _mapper.MapCreateAdUserArguments(command));

                builder.AddActivity(UserServiceConstants.ActivityCreateIsUser,
                    _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateIdentityServerUserActivity)),
                    _mapper.MapCreateIsUserArguments(command));
            }

            this.Log().Info($"03 - Create Office 365 Database User: {command.UserPrincipalName}");

            builder.AddActivity(Office365ServiceConstants.SetImmutableId,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ISetImmutableIdActivity)),
                new
                {
                    command.Office365CustomerId,
                    command.UserPrincipalName
                });
            
            builder.AddActivity(Office365ServiceConstants.ActivityCreateOffice365DatabaseUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDatabaseUserActivity)),
                _mapper.MapCreateOffice365DatabaseUserArguments(command));
            
            if (!command.KeepLicences)
            {
                if (command.Admin)
                {
                    this.Log().Info($"04 - Enter in Admin: {command.UserPrincipalName}");

                    builder.AddActivity(Office365ServiceConstants.ActivityGetUserRoles,
                        _activityConfigurator.GetActivityExecuteUri(context, typeof(IGetUserRolesActivity)),
                        _mapper.MapGetUserRolesArguments(command));


                    builder.AddActivity(Office365ServiceConstants.ActivityRemoveUserRoles,
                        _activityConfigurator.GetActivityExecuteUri(context, typeof(IRemoveUserRolesActivity)),
                        _mapper.MapRemoveUserRolesArguments(command));

                    builder.AddActivity(Office365ServiceConstants.ActivityAssignUserRoles,
                        _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignUserRolesActivity)),
                        _mapper.MapAssignUserRolesArguments(command));
                }

                this.Log().Info($"05 - Remove All Licenses Partner Portal User: {command.UserPrincipalName}");

                builder.AddActivity(Office365ServiceConstants.ActivityRemoveAllLicensesPartnerPortalUser,
                    _activityConfigurator.GetActivityExecuteUri(context,
                        typeof(IRemoveAllLicensesPartnerPortalUserActivity)),
                    _mapper.MapRemoveAllLicensesPartnerPortalUserArguments(command));

                if (command.IsNewLicenses)
                {
                    this.Log().Info($"06 - Enter in IsNewLicenses: {command.UserPrincipalName}");

                    if (command.CloudPlusProductIdentifier != null)
                    {
                        builder.AddActivity(Office365ServiceConstants.ActivityAssignLicenseOffice365PartnerPlatformUser,
                            _activityConfigurator.GetActivityExecuteUri(context,
                                typeof(IAssignLicenseToPartnerPlatformUserActivity)),
                            _mapper.MapAssignLicenseOffice365PartnerPlatformUserArguments(command));

                        builder.AddActivity(Office365ServiceConstants.ActivityAssignLicenseOffice365DatabaseUser,
                            _activityConfigurator.GetActivityExecuteUri(context,
                                typeof(IAssignLicenseToDatabaseUserActivity)),
                            _mapper.MapAssignLicenseOffice365DatabaseUserArguments(command));
                    }
                }
            }

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
                WorkflowActivityType = WorkflowActivityType.Office365TransitionUserAndLicenses.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}