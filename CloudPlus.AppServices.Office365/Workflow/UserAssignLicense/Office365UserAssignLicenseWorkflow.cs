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
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Database.Office365.User;
using CloudPlus.Services.Office365.User;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToPartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.AssignUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.CreateDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.CreatePartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.SendUserSetupEmail;
using CloudPlus.Workflows.Office365.Mappers;

namespace CloudPlus.AppServices.Office365.Workflow.UserAssignLicense
{
    public class Office365UserAssignLicenseWorkflow : IOffice365UserAssignLicenseWorkflow
    {
        private readonly IActivityOffice365UserArgumentsMapper _activityOffice365UserArgumentsMapper;
        private readonly IActivityConfigurator _activityConfigurator;
        private readonly IOffice365DbUserService _office365DbUserService;
        private readonly IOffice365UserService _office365UserService;
        private readonly IOffice365DbCustomerService _office365DbCustomerService;

        public Office365UserAssignLicenseWorkflow(
            IActivityOffice365UserArgumentsMapper activityOffice365UserArgumentsMapper,
            IActivityConfigurator activityConfigurator, IOffice365UserService office365UserService,
            IOffice365DbUserService office365DbUserService, IOffice365DbCustomerService office365DbCustomerService)
        {
            _activityOffice365UserArgumentsMapper = activityOffice365UserArgumentsMapper;
            _activityConfigurator = activityConfigurator;
            _office365UserService = office365UserService;
            _office365DbUserService = office365DbUserService;
            _office365DbCustomerService = office365DbCustomerService;
        }

        public async Task Execute(ConsumeContext<IOffice365UserAssignLicenseCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var command = context.Message;

            var customer = await _office365DbCustomerService.GetOffice365CustomerAsync(command.CompanyId);
            
            var o365UserId =
                await _office365UserService.GetOffice365UserIdAsync(command.UserPrincipalName,
                    customer.Office365CustomerId);
            
            var o365DbUser = await _office365DbUserService.GetOffice365DatabaseUserAsync(command.UserPrincipalName);
            
            builder.AddVariable("Office365CustomerId", customer.Office365CustomerId);
            
            if (o365UserId == null)
            {
                builder.AddActivity(Office365ServiceConstants.ActivityCreateOffice365PartnerPlatformUser,
                    _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreatePartnerPlatformUserActivity)),
                    _activityOffice365UserArgumentsMapper.MapCreatePartnerPlatformUserArguments(command));
            }
                
            if (o365DbUser == null)
            {
                if (o365UserId != null)
                {
                    builder.AddVariable("Office365UserId", o365UserId);
                }
                
                builder.AddActivity(Office365ServiceConstants.ActivityCreateOffice365DatabaseUser,
                    _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDatabaseUserActivity)),
                    _activityOffice365UserArgumentsMapper.MapCreateOffice365DatabaseUserArguments(command));
            }

            builder.AddActivity(Office365ServiceConstants.ActivityAssignLicenseOffice365PartnerPlatformUser,
                _activityConfigurator.GetActivityExecuteUri(context,
                    typeof(IAssignLicenseToPartnerPlatformUserActivity)),
                _activityOffice365UserArgumentsMapper.MapAssignLicenseOffice365PartnerPlatformUserArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityAssignLicenseOffice365DatabaseUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignLicenseToDatabaseUserActivity)),
                _activityOffice365UserArgumentsMapper.MapAssignLicenseOffice365DatabaseUserArguments(command));

            if (command.UserRoles.Any())
            {
                builder.AddActivity(Office365ServiceConstants.ActivityAssignUserRoles,
                    _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignUserRolesActivity)),
                    _activityOffice365UserArgumentsMapper.MapAssignUserRolesArguments(command));
            }

            builder.AddActivity(Office365ServiceConstants.ActivitySendOffice365UserSetupEmail,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ISendUserSetupEmailActivity)),
                _activityOffice365UserArgumentsMapper.MapSendOffice365UserSetupEmailArguments(command));

            builder.AddVariable("userPrincipalName", context.Message.UserPrincipalName);
            builder.AddVariable("workflowActivityType", WorkflowActivityType.Office365UserAssignLicense.ToString());

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
                WorkflowActivityType = WorkflowActivityType.Office365UserAssignLicense.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}
