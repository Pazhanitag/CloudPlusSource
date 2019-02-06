using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using CloudPlus.Constants;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToPartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.AssignUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.GetUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.RemoveLicenseDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.RemoveLicensePartnerPortalUser;
using CloudPlus.Workflows.Office365.Activities.User.RemoveUserRoles;
using CloudPlus.Workflows.Office365.Mappers;

namespace CloudPlus.AppServices.Office365.Workflow.UserChangeLicense
{
    public class Office365UserChangeLicenseWorkflow : IOffice365UserChangeLicenseWorkflow
    {
        private readonly IActivityOffice365UserChangeLicenseMapper _mapper;
        private readonly IActivityConfigurator _activityConfigurator;

        public Office365UserChangeLicenseWorkflow(IActivityOffice365UserChangeLicenseMapper mapper, IActivityConfigurator activityConfigurator)
        {
            _mapper = mapper;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<IOffice365UserChangeLicenseCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var command = context.Message;

            builder.AddActivity(Office365ServiceConstants.ActivityRemoveLicensePartnerPortalUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IRemoveLicensePartnerPortalUserActivity)),
                _mapper.MapRemoveLicensePartnerPortalUserArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityRemoveLicenseDatabaseUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IRemoveLicenseDatabaseUserActivity)),
                _mapper.MapRemoveLicenseDatabaseUserArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityAssignLicenseOffice365PartnerPlatformUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignLicenseToPartnerPlatformUserActivity)),
                _mapper.MapAssignLicenseOffice365PartnerPlatformUserArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityAssignLicenseOffice365DatabaseUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignLicenseToDatabaseUserActivity)),
                _mapper.MapAssignLicenseOffice365DatabaseUserArguments(command));

            if (command.UserRoles.Any())
            {
                builder.AddActivity(Office365ServiceConstants.ActivityGetUserRoles,
                    _activityConfigurator.GetActivityExecuteUri(context, typeof(IGetUserRolesActivity)), _mapper.MapGetUserRolesArguments(command));

                builder.AddActivity(Office365ServiceConstants.ActivityRemoveUserRoles,
                    _activityConfigurator.GetActivityExecuteUri(context, typeof(IRemoveUserRolesActivity)), _mapper.MapRemoveUserRolesArguments(command));

                builder.AddActivity(Office365ServiceConstants.ActivityAssignUserRoles,
                    _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignUserRolesActivity)), _mapper.MapAssignUserRolesArguments(command));
            }

            builder.AddSubscription(Office365ServiceConstants.RoutingSlipEventObserverUri,
                RoutingSlipEvents.ActivityCompleted |
                RoutingSlipEvents.ActivityFaulted |
                RoutingSlipEvents.ActivityCompensated |
                RoutingSlipEvents.ActivityCompensationFailed);

            var routingSlip = builder.Build();

            await context.Execute(routingSlip);
        }
    }
}
