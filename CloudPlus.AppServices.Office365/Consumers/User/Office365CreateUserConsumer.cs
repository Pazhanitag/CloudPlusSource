using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using CloudPlus.Constants;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Common;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Services.Identity.User;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Office365.Activities.User.AssignUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.CreateDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.CreatePartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.SendUserSetupEmail;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public class Office365CreateUserConsumer : IOffice365CreateUserConsumer
    {
        private readonly IUserService _userService;
        private readonly IActivityConfigurator _activityConfigurator;

        public Office365CreateUserConsumer(IUserService userService, IActivityConfigurator activityConfigurator)
        {
            _userService = userService;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Consume(ConsumeContext<IOffice365UserCreateCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var command = context.Message;

            var user = _userService.GetUser(command.UserPrincipalName);

            if (user == null)
                throw new Exception($"User {command.UserPrincipalName} does not exist in system");
            
            builder.AddActivity(Office365ServiceConstants.ActivityCreateOffice365PartnerPlatformUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreatePartnerPlatformUserActivity)),
                new
                {
                    command.CompanyId,
                    command.UserPrincipalName,
                    user.DisplayName,
                    user.FirstName,
                    user.LastName,
                    command.UsageLocation,
                    user.City,
                    user.Country,
                    user.PhoneNumber,
                    PostalCode = user.ZipCode,
                    user.State,
                    command.Password,
                    user.StreetAddress,
                    WorkflowActivityType = WorkflowActivityType.Office365CreateUser,
                    WorkflowStep = WorkflowActivityStep.Office365CreatePartnerPlatformUser
                });

            builder.AddActivity(Office365ServiceConstants.ActivityCreateOffice365DatabaseUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDatabaseUserActivity)),
                new
                {
                    command.UserPrincipalName,
                    WorkflowActivityType = WorkflowActivityType.Office365CreateUser,
                    WorkflowStep = WorkflowActivityStep.Office365CreateDatabaseUser
                });

            if (command.UserRoles.Any())
            {

                builder.AddActivity(Office365ServiceConstants.ActivityAssignUserRoles,
                    _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignUserRolesActivity)),
                    new
                    {
                        command.UserPrincipalName,
                        command.UserRoles,
                        WorkflowActivityType = WorkflowActivityType.Office365CreateUser,
                        WorkflowStep = WorkflowActivityStep.Office365AssignUserRoles
                    });
            }

            builder.AddActivity(Office365ServiceConstants.ActivitySendOffice365UserSetupEmail,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ISendUserSetupEmailActivity)),
                new
                {
                    command.UserPrincipalName,
                    WorkflowActivityType = WorkflowActivityType.Office365CreateUser,
                    WorkflowStep = WorkflowActivityStep.Office365SendUserSetupEmail
                });

            var routingSlip = builder.Build();

            await context.Send<IRoutingSlipStarted>(Office365ServiceConstants.QueueOffice365RoutingSlipStartedUri, new
            {
                builder.TrackingNumber,
                CreateTimestamp = DateTime.UtcNow,
                Arguments = context.Message,
                WorkflowActivityType = WorkflowActivityType.Office365CreateUser.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}
