using System;

namespace CloudPlus.Constants
{
    public static class UserServiceConstants
    {
        public static string RabbitMqUri = System.Configuration.ConfigurationManager.AppSettings["RabbitMqUri"];
        public static string CompensateSuffix = "compensate";
        
        // Events
        public static string RoutingSlipEventObserverRoute = "user-service-routing-slip-event-observer";
        public static Uri RoutingSlipEventObserverUri = new Uri($"{RabbitMqUri}{RoutingSlipEventObserverRoute}");

        public static string RoutingSlipUserStartedEventRoute = "user-routing-slip-started";
        public static Uri RoutingSlipUserStartedEventUri = new Uri($"{RabbitMqUri}{RoutingSlipUserStartedEventRoute}");

        // Create User
        public static string QueueCreateUser = "create-user-flow";
        public static string ActivityCreateAdUser = "CreateAdUser";
        public static string ActivityCreateIsUser = "CreateIsUser";
        public static string ActivityCreateAdUserRoute = "create-ad-user-activity";
        public static string ActivityCreateIsUserRoute = "create-is-user-activity";
        public static string CompensateCreateAdUserRoute = $"{ActivityCreateAdUserRoute}-{CompensateSuffix}";
        public static string CompensateCreateIsUserRoute = $"{ActivityCreateIsUserRoute}-{CompensateSuffix}";
	    public static Uri CreateUserUri = new Uri($"{RabbitMqUri}{QueueCreateUser}");
        public static Uri CreateAdUserUri = new Uri($"{RabbitMqUri}{ActivityCreateAdUserRoute}");
        public static Uri CreateIsUserUri = new Uri($"{RabbitMqUri}{ActivityCreateIsUserRoute}");
        public static Uri CompensateCreateAdUserUri => new Uri($"{RabbitMqUri}{CompensateCreateAdUserRoute}");
        public static Uri CompensateCreateIsUserUri => new Uri($"{RabbitMqUri}{CompensateCreateIsUserRoute}");

        // Created User send email
        public static string ActivityUserCreated = "UserCreated";
        public static string ActivityUserCreatedRoute = "user-created-activity";
        public static Uri UserCreatedUri = new Uri($"{RabbitMqUri}{ActivityUserCreatedRoute}");

        // Update user
        public static string QueueUpdateUser = "update-user-flow";
        public static string ActivityUpdateAdUser = "UpdateAdUser";
        public static string ActivityUpdateIsUser = "UpdateIsUser";
        public static string ActivityUpdateAdUserRoute = "update-ad-user-activity";
        public static string ActivityUpdateIsUserRoute = "update-is-user-activity";
        public static string CompensateUpdateAdUserRoute = $"{ActivityUpdateAdUserRoute}-{CompensateSuffix}";
        public static Uri UpdateAdUserUri = new Uri($"{RabbitMqUri}{ActivityUpdateAdUserRoute}");
        public static Uri UpdateIsUserUri = new Uri($"{RabbitMqUri}{ActivityUpdateIsUserRoute}");
        public static Uri CompensateUpdateAdUserUri = new Uri($"{RabbitMqUri}{CompensateUpdateAdUserRoute}");

        // Change User password
        public static string QueueChangeUserPassword = "change-user-password-flow";

		// Delete User
	    public static string QueueDeleteUser = "delete-user-flow";
	    public static string ActivityDeleteAdUser = "DeleteAdUser";
	    public static string ActivityDeleteIsUser = "DeleteIsUser";
	    public static string ActivityDeleteAdUserRoute = "delete-ad-user-activity";
	    public static string ActivityDeleteIsUserRoute = "delete-is-user-activity";
		public static Uri DeleteAdUserUri = new Uri($"{RabbitMqUri}{ActivityDeleteAdUserRoute}");
	    public static Uri DeleteIsUserUri = new Uri($"{RabbitMqUri}{ActivityDeleteIsUserRoute}");
	    public static string CompensateDeleteAdUserRoute = $"{ActivityDeleteAdUserRoute}-{CompensateSuffix}";
	    public static string CompensateDeleteIsUserRoute = $"{ActivityDeleteIsUserRoute}-{CompensateSuffix}";
		public static Uri CompensateDeleteAdUserUri => new Uri($"{RabbitMqUri}{CompensateDeleteAdUserRoute}");
	    public static Uri CompensateDeleteIsUserUri => new Uri($"{RabbitMqUri}{CompensateDeleteIsUserRoute}");

        //Groups
        public static string QueueCreateSecurityGroup = "create-security-group-flow";
        public static Uri QueueCreateSecurityGroupUri => new Uri($"{RabbitMqUri}{QueueCreateSecurityGroup}");

        public static string QueueCreateDistributionGroup = "create-distribution-group-flow";
        public static Uri QueueCreateDistributionGroupUri => new Uri($"{RabbitMqUri}{QueueCreateDistributionGroup}");

        public static string QueueCreateOffice365Group = "create-office365-group-flow";
        public static Uri QueueCreateOffice365GroupUri => new Uri($"{RabbitMqUri}{QueueCreateOffice365Group}");

        public static string QueueRemoveSecurityGroup = "delete-security-group-flow";
        public static Uri QueueRemoveSecurityGroupUri => new Uri($"{RabbitMqUri}{QueueRemoveSecurityGroup}");

        public static string QueueRemoveDistributionGroup = "delete-distribution-group-flow";
        public static Uri QueueRemoveDistributionGroupUri => new Uri($"{RabbitMqUri}{QueueRemoveDistributionGroup}");

        public static string QueueRemoveOffice365Group = "delete-office365-group-flow";
        public static Uri QueueRemoveOffice365GroupUri => new Uri($"{RabbitMqUri}{QueueRemoveOffice365Group}");

    }
}
