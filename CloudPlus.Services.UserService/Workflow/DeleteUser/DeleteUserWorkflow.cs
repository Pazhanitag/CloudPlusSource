using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using CloudPlus.Constants;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Common;
using CloudPlus.QueueModels.Office365.Subscriptions.Commands;
using CloudPlus.QueueModels.Users.Commands;
using CloudPlus.Services.Identity.User;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.User.Activities.DeleteActiveDirectoryUser;
using CloudPlus.Workflows.User.Activities.DeleteIdentityServerUser;
using CloudPlus.Workflows.User.Mappers;

namespace CloudPlus.AppServices.User.Workflow.DeleteUser
{
    public class DeleteUserWorkflow : IDeleteUserWorkflow
	{
		private readonly IActivityDeleteUserArgumentsMapper _activityArgumentsMapper;
	    private readonly IActivityConfigurator _activityConfigurator;
	    private readonly IUserService _userService;

        public DeleteUserWorkflow(IActivityDeleteUserArgumentsMapper activityArgumentsMapper, IActivityConfigurator activityConfigurator, IUserService userService)
	    {
	        _activityArgumentsMapper = activityArgumentsMapper;
	        _activityConfigurator = activityConfigurator;
	        _userService = userService;
	    }

		public async Task Execute(ConsumeContext<IDeleteUserCommand> context)
		{
			var builder = new RoutingSlipBuilder(NewId.NextGuid());
			var deleteUserCommand = context.Message;

			builder.AddActivity(UserServiceConstants.ActivityDeleteAdUser, _activityConfigurator.GetActivityExecuteUri(context, typeof(IDeleteActiveDirectoryUserActivity)), _activityArgumentsMapper.MapDeleteActiveDirectoryUserArguments(deleteUserCommand));
			builder.AddActivity(UserServiceConstants.ActivityDeleteIsUser, _activityConfigurator.GetActivityExecuteUri(context, typeof(IDeleteIdentityServerUserActivity)), _activityArgumentsMapper.MapDeleteIdentityServerUserArguments(deleteUserCommand));

		    builder.AddSubscription(UserServiceConstants.RoutingSlipEventObserverUri,
		        RoutingSlipEvents.Completed |
		        RoutingSlipEvents.Faulted |
		        RoutingSlipEvents.ActivityCompleted |
		        RoutingSlipEvents.ActivityFaulted |
		        RoutingSlipEvents.ActivityCompensated |
		        RoutingSlipEvents.ActivityCompensationFailed);

            var user = _userService.GetUser(context.Message.UserId);

            await builder.AddSubscription(Office365ServiceConstants.QueueManageSubscriptionsAndLicencesUri,
                RoutingSlipEvents.Completed,
                x => x.Send<IManageSubscriptionsAndLicencesCommand>(new
                {
                    context.Message.CompanyId,
                    Users = new List<Office365LicenceUser>
                    {
                              new Office365LicenceUser
                              {
                                  UserPrincipalName = user.Email
                              }
                    },
                    MessageType = ManageSubsctiptionAndLicenceCommandType.HardDeleteUser
                }));


            var routingSlip = builder.Build();

		    await context.Send<IRoutingSlipStarted>(UserServiceConstants.RoutingSlipUserStartedEventUri, new
		    {
		        builder.TrackingNumber,
		        CreateTimestamp = DateTime.UtcNow,
		        Arguments = context.Message,
		        WorkflowActivityType = WorkflowActivityType.DeleteUser.ToString()
            });

            await context.Execute(routingSlip);
		}
	}
}
