using System.Threading.Tasks;
using CloudPlus.Constants;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.QueueModels.Users.Commands;
using CloudPlus.Services.Identity.User;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.User.Activities.DeleteActiveDirectoryUser;
using CloudPlus.Workflows.User.Activities.DeleteIdentityServerUser;
using CloudPlus.Workflows.User.Mappers;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;

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
				RoutingSlipEvents.ActivityCompleted |
				RoutingSlipEvents.ActivityFaulted |
				RoutingSlipEvents.ActivityCompensated |
				RoutingSlipEvents.ActivityCompensationFailed);

		    var user = _userService.GetUser(context.Message.UserId);
		    await builder.AddSubscription(Office365ServiceConstants.QueueOffice365UserRemoveLicenseUri,
		        RoutingSlipEvents.Completed,
		        x => x.Send<IOffice365UserRemoveLicenseCommand>(new
		        {
		            context.Message.CompanyId,
		            UserPrincipalName = user.Email
		        }));

            var routingSlip = builder.Build();

			await context.Execute(routingSlip);
		}
	}
}
