using System;

namespace CloudPlus.Constants
{
    public static class Office365ServiceConstants
    {
        public static string RabbitMqUri = System.Configuration.ConfigurationManager.AppSettings["RabbitMqUri"];
        public static string CompensateSuffix = "compensate";

        // Events

        public static string RoutingSlipEventObserverRoute = "office365-service-routing-slip-event-observer";
        public static Uri RoutingSlipEventObserverUri = new Uri($"{RabbitMqUri}{RoutingSlipEventObserverRoute}");

        public static string QueueOffice365RoutingSlipEventRoute = "queue-office365-service-routing-slip-event-observer";
        public static Uri QueueOffice365RoutingSlipEventUri = new Uri($"{RabbitMqUri}{QueueOffice365RoutingSlipEventRoute}");

        public static string QueueOffice365RoutingSlipStartedRoute = "office365-service-routing-slip-started";
        public static Uri QueueOffice365RoutingSlipStartedUri = new Uri($"{RabbitMqUri}{QueueOffice365RoutingSlipStartedRoute}");

        public static string Office365ManageSubscriptionsAndLicencesObserverRoute = "office365-manage-subscription-and-licenses-observer";

        public static string QueueOffice365TransitionDeletePartnerPlatformUserObserverRoute = "office365-transition-delte-partner-platform-user-observer";

        // Office 365 Address validation
        public static string QueueOffice365AddressValidation = "office-365-address-validation";
        public static Uri Office365AddressValidationUri = new Uri($"{RabbitMqUri}{QueueOffice365AddressValidation}");

        // Office 365 Customer
        public static string QueueCreateOffice365Customer = "create-office365-customer-flow";

        public static string ActivityCreateOffice365PartnerPlatformCustomer = "ActivityCreateOffice365PartnerPlatformCustomer";
        public static string ActivityCreateOffice365PartnerPlatformCustomerRoute = "activity-create-office365-partner-platform-customer";
        public static Uri CreateOffice365PartnerPlatformCustomerUri = new Uri($"{RabbitMqUri}{ActivityCreateOffice365PartnerPlatformCustomerRoute}");
        public static string CompensateCreateOffice365PartnerPlatformCustomerRoute = $"{ActivityCreateOffice365PartnerPlatformCustomerRoute}-{CompensateSuffix}";
        public static Uri CompensateCreateOffice365PartnerPlatformCustomerUri = new Uri($"{RabbitMqUri}{CompensateCreateOffice365PartnerPlatformCustomerRoute}");

        public static string ActivityCreateOffice365DatabaseCustomer = "ActivityCreateOffice365DatabaseCustomer";
        public static string ActivityCreateOffice365DatabaseCustomerRoute = "activity-create-office365-database-customer";
        public static Uri CreateOffice365DatabaseCustomerUri = new Uri($"{RabbitMqUri}{ActivityCreateOffice365DatabaseCustomerRoute}");
        public static string CompensateCreateOffice365DatabaseCustomerRoute = $"{ActivityCreateOffice365DatabaseCustomerRoute}-{CompensateSuffix}";
        public static Uri CompensateCreateOffice365DatabaseCustomerUri = new Uri($"{RabbitMqUri}{CompensateCreateOffice365DatabaseCustomerRoute}");

        public static string ActivityPartnerPlatformOffice365CustomerSubscription = "ActivityPartnerPlatformOffice365CustomerSubscription";
        public static string ActivityPartnerPlatformOffice365CustomerSubscriptionRoute = "activity-partner-portal-office365-customer-subscription";
        public static Uri PartnerPlatformOffice365CustomerSubscriptionUri = new Uri($"{RabbitMqUri}{ActivityPartnerPlatformOffice365CustomerSubscriptionRoute}");
        public static string CompensatePartnerPlatformOffice365CustomerSubscriptionRoute = $"{ActivityPartnerPlatformOffice365CustomerSubscriptionRoute}-{CompensateSuffix}";
        public static Uri CompensatePartnerPlatformOffice365CustomerSubscriptionUri = new Uri($"{RabbitMqUri}{CompensatePartnerPlatformOffice365CustomerSubscriptionRoute}");

        public static string ActivityMultiPartnerPlatformCustomerSubscription = "ActivityMultiPartnerPlatformCustomerSubscription";
        public static string ActivityMultiPartnerPlatformCustomerSubscriptionRoute = "activity-office365-multi-partner-platform-customer-subscription";
        public static Uri MultiPartnerPlatformCustomerSubscriptionUri = new Uri($"{RabbitMqUri}{ActivityMultiPartnerPlatformCustomerSubscriptionRoute}");
        public static string CompensateMultiPartnerPlatformCustomerSubscriptionRoute = $"{ActivityMultiPartnerPlatformCustomerSubscriptionRoute}-{CompensateSuffix}";
        public static Uri CompensateMultiPartnerPlatformCustomerSubscriptionUri = new Uri($"{RabbitMqUri}{CompensateMultiPartnerPlatformCustomerSubscriptionRoute}");

        public static string ActivityDecreasePartnerPlatformCustomerSubscription = "ActivityDecreasePartnerPlatformCustomerSubscription";
        public static string ActivityDecreasePartnerPlatformCustomerSubscriptionRoute = "activity-decrease-partner-platform-customer-subscription";
        public static Uri DecreasePartnerPlatformCustomerSubscriptionUri = new Uri($"{RabbitMqUri}{ActivityDecreasePartnerPlatformCustomerSubscriptionRoute}");
        public static string CompensateDecreasePartnerPlatformCustomerSubscriptionRoute = $"{ActivityDecreasePartnerPlatformCustomerSubscriptionRoute}-{CompensateSuffix}";
        public static Uri CompensateDecreasePartnerPlatformCustomerSubscriptionUri = new Uri($"{RabbitMqUri}{CompensateDecreasePartnerPlatformCustomerSubscriptionRoute}");

        public static string ActivityDatabaseOffice365CustomerSubscription = "ActivityDatabaseOffice365CustomerSubscription";
        public static string ActivityDatabaseOffice365CustomerSubscriptionRoute = "activity-database-office365-customer-subscription";
        public static Uri DatabaseOffice365CustomerSubscriptionUri = new Uri($"{RabbitMqUri}{ActivityDatabaseOffice365CustomerSubscriptionRoute}");
        public static string CompensateDatabaseOffice365CustomerSubscriptionRoute = $"{ActivityDatabaseOffice365CustomerSubscriptionRoute}-{CompensateSuffix}";
        public static Uri CompensateDatabaseOffice365CustomerSubscriptionUri = new Uri($"{RabbitMqUri}{CompensateDatabaseOffice365CustomerSubscriptionRoute}");

        public static string ActivityUpdateDatabaseOffice365CustomerSubscription = "ActivityUpdateDatabaseOffice365CustomerSubscription";
        public static string ActivityUpdateDatabaseOffice365CustomerSubscriptionRoute = "activity-update-database-office365-customer-subscription";
        public static Uri UpdateDatabaseOffice365CustomerSubscriptionUri = new Uri($"{RabbitMqUri}{ActivityUpdateDatabaseOffice365CustomerSubscriptionRoute}");

        public static string ActivityMultiDatabaseCustomerSubscription = "ActivityMultiDatabaseCustomerSubscription";
        public static string ActivityMultiDatabaseCustomerSubscriptionRoute = "activity-office365-multi-database-customer-subscription";
        public static Uri MultiDatabaseCustomerSubscriptionUri = new Uri($"{RabbitMqUri}{ActivityMultiDatabaseCustomerSubscriptionRoute}");
        public static string CompensateMultiDatabaseCustomerSubscriptionRoute = $"{ActivityMultiDatabaseCustomerSubscriptionRoute}-{CompensateSuffix}";
        public static Uri CompensateMultiDatabaseCustomerSubscriptionUri = new Uri($"{RabbitMqUri}{CompensateMultiDatabaseCustomerSubscriptionRoute}");
        
        public static string ActivityDecreaseDatabaseCustomerSubscription = "ActivityDecreaseDatabaseCustomerSubscription";
        public static string ActivityDecreaseDatabaseCustomerSubscriptionRoute = "activity-decrease-database-customer-subscription";
        public static Uri DecreaseDatabaseCustomerSubscriptionUri = new Uri($"{RabbitMqUri}{ActivityDecreaseDatabaseCustomerSubscriptionRoute}");
        public static string CompensateDecreaseDatabaseCustomerSubscriptionRoute = $"{ActivityDecreaseDatabaseCustomerSubscriptionRoute}-{CompensateSuffix}";
        public static Uri CompensateDecreaseDatabaseCustomerSubscriptionUri = new Uri($"{RabbitMqUri}{CompensateDecreaseDatabaseCustomerSubscriptionRoute}");

        // Create Office 365 User
        public static string QueueOffice365UserAssignLicense = "office365-user-assign-license-flow";
        public static Uri QueueOffice365UserAssignLicenseUri => new Uri($"{RabbitMqUri}{QueueOffice365UserAssignLicense}");
        public static string QueueOffice365UserRemoveLicense = "office365-user-remove-license-flow";

        public static Uri QueueOffice365UserRemoveLicenseUri => new Uri($"{RabbitMqUri}{QueueOffice365UserRemoveLicense}");

        public static string QueueOffice365HardDeleteUser = "office365-hard-delete-user";
        public static Uri QueueOffice365HardDeleteUserUri => new Uri($"{RabbitMqUri}{QueueOffice365HardDeleteUser}");

        public static string QueueOffice365UserRestore = "office365-user-restore-flow";
        public static Uri QueueOffice365UserRestoreUri => new Uri($"{RabbitMqUri}{QueueOffice365UserRestore}");

        public static string QueueOffice3655UserMultiEdit = "office365-user-multi-edit-flow";
        public static Uri Office3655UserMultiEditUri => new Uri($"{RabbitMqUri}{QueueOffice3655UserMultiEdit}");
        public static string QueueOffice365UserChangeLicense = "office365-user-change-license-flow";
        public static Uri Office365UserChangeLicenseUri => new Uri($"{RabbitMqUri}{QueueOffice365UserChangeLicense}");
        public static string QueueOffice365GetUserRoles = "office-365-get-user-roles";
        public static Uri QueueOffice365GetUserRolesUri = new Uri($"{RabbitMqUri}{QueueOffice365GetUserRoles}");
        public static string QueueOffice365ChangeUserRoles = "office-365-change-user-roles";
        public static Uri QueueOffice365ChangeUserRolesUri = new Uri($"{RabbitMqUri}{QueueOffice365ChangeUserRoles}");
        public static string QueueOffice365CreateUser = "office365-create-user";
        public static Uri QueueOffice365CreateUserUri => new Uri($"{RabbitMqUri}{QueueOffice365CreateUser}");

        public static string ActivityCreateOffice365PartnerPlatformUser = "ActivityCreateOffice365PartnerPlatformUser";
        public static string ActivityCreateOffice365PartnerPlatformUserRoute = "activity-create-office365-partner-platform-user";
        public static Uri CreateOffice365PartnerPlatformUserUri = new Uri($"{RabbitMqUri}{ActivityCreateOffice365PartnerPlatformUserRoute}");
        public static string CompensateCreateOffice365PartnerPlatformUserRoute = $"{ActivityCreateOffice365PartnerPlatformUserRoute}-{CompensateSuffix}";
        public static Uri CompensateCreateOffice365PartnerPlatformUserUri = new Uri($"{RabbitMqUri}{CompensateCreateOffice365PartnerPlatformUserRoute}");

        public static string ActivityCreateTempAdminUser = "ActivityCreateTempAdminUser";
        public static string ActivityCreateTempAdminUserRoute = "activity-create-office365-temp-admin-user";
        public static Uri CreateTempAdminUserUri = new Uri($"{RabbitMqUri}{ActivityCreateTempAdminUserRoute}");

        public static string ActivityCreateOffice365DatabaseUser = "ActivityCreateOffice365DatabaseUser";
        public static string ActivityCreateOffice365DatabaseUserRoute = "activity-create-office365-database-user";
        public static Uri CreateOffice365DatabaseUserUri = new Uri($"{RabbitMqUri}{ActivityCreateOffice365DatabaseUserRoute}");
        public static string CompensateCreateOffice365DatabaseUserRoute = $"{ActivityCreateOffice365DatabaseUserRoute}-{CompensateSuffix}";
        public static Uri CompensateCreateOffice365DatabaseUserUri = new Uri($"{RabbitMqUri}{CompensateCreateOffice365DatabaseUserRoute}");

        public static string ActivityHardDeletePartnerPlatformUser = "ActivityHardDeletePartnerPlatformUser";
        public static string ActivityHardDeletePartnerPlatformUserRoute = "activity-hard-delete-partner-platform-office365-user";
        public static Uri HardDeletePartnerPlatformUserUri = new Uri($"{RabbitMqUri}{ActivityHardDeletePartnerPlatformUserRoute}");

        public static string ActivityHardDeleteDatabaseUser = "ActivityHardDeleteDatabaseUser";
        public static string ActivityHardDeleteDatabaseUserRoute = "activity-hard-delete-database-office365-user";
        public static Uri HardDeleteDatabaseUserUri = new Uri($"{RabbitMqUri}{ActivityHardDeleteDatabaseUserRoute}");

        public static string ActivityDeletePartnerPlatformUser = "ActivityDeletePartnerPlatformUserActivity";
        public static string ActivityDeletePartnerPlatformUserRoute = "activity-delete-office365-partner-platform-user";
        public static Uri DeletePartnerPlatformUserUri = new Uri($"{RabbitMqUri}{ActivityDeletePartnerPlatformUserRoute}");
        public static string CompensateDeletePartnerPlatformUserRoute = $"{ActivityDeletePartnerPlatformUserRoute}-{CompensateSuffix}";
        public static Uri CompensateDeletePartnerPlatformUserUri = new Uri($"{RabbitMqUri}{CompensateDeletePartnerPlatformUserRoute}");

        public static string ActivityRestorePartnerPlatformUser = "ActivityOffice365UserRestore";
        public static string ActivityRestorePartnerPlatformUserRoute = "activity-restore-office365-partner-portal-user";
        public static Uri RestorePartnerPlatformUserUri = new Uri($"{RabbitMqUri}{ActivityRestorePartnerPlatformUserRoute}");
        public static string CompensateRestorePartnerPlatformUserRoute = $"{ActivityRestorePartnerPlatformUserRoute}-{CompensateSuffix}";
        public static Uri CompensateRestorePartnerPlatformUserUri = new Uri($"{RabbitMqUri}{CompensateRestorePartnerPlatformUserRoute}");

        public static string ActivityDeleteDatabaseUser = "ActivityDeleteDatabaseUser";
        public static string ActivityDeleteDatabaseUserRoute = "activity-delete-database-office365-user";
        public static Uri DeleteDatabaseUserUri = new Uri($"{RabbitMqUri}{ActivityDeleteDatabaseUserRoute}");

        public static string ActivitySoftDeleteDatabaseUser = "ActivitySoftDeleteDatabaseUser";
        public static string ActivitySoftDeleteDatabaseUserRoute = "activity-soft-delete-database-office365-user";
        public static Uri SoftDeleteDatabaseUserUri = new Uri($"{RabbitMqUri}{ActivitySoftDeleteDatabaseUserRoute}");
        public static string CompensateSoftDeleteDatabaseUserRoute = $"{ActivitySoftDeleteDatabaseUserRoute}-{CompensateSuffix}";
        public static Uri CompensateSoftDeleteDatabaseUserUri = new Uri($"{RabbitMqUri}{CompensateSoftDeleteDatabaseUserRoute}");

        public static string ActivityActivateSoftDeletedDatabaseUser = "ActivityActivateSoftDeletedDatabaseUser";
        public static string ActivityActivateSoftDeletedDatabaseUserRoute = "activity-activate-soft-deleted-office365-database-user";
        public static Uri ActivateSoftDeletedDatabaseUserUri = new Uri($"{RabbitMqUri}{ActivityActivateSoftDeletedDatabaseUserRoute}");
        public static string CompensateActivateSoftDeletedDatabaseUserRoute = $"{ActivityActivateSoftDeletedDatabaseUserRoute}-{CompensateSuffix}";
        public static Uri CompensateActivateSoftDeletedDatabaseUserUri = new Uri($"{RabbitMqUri}{CompensateActivateSoftDeletedDatabaseUserRoute}");

        public static string ActivityAssignLicenseOffice365PartnerPlatformUser = "ActivityAssignLicenseOffice365PartnerPlatformUser";
        public static string ActivityAssignLicenseOffice365PartnerPlatformUserRoute = "activity-assign-license-office365-partner-platform-user";
        public static Uri AssignLicenseOffice365PartnerPlatformUserUri = new Uri($"{RabbitMqUri}{ActivityAssignLicenseOffice365PartnerPlatformUserRoute}");

        public static string ActivityAssignLicenseOffice365DatabaseUser = "ActivityAssignLicenseOffice365DatabaseUser";
        public static string ActivityAssignLicenseOffice365DatabaseUserRoute = "activity-assign-license-office365-database-user";
        public static Uri AssignLicenseOffice365DatabaseUserUri = new Uri($"{RabbitMqUri}{ActivityAssignLicenseOffice365DatabaseUserRoute}");

        public static string ActivityRemoveLicenseDatabaseUser = "ActivityRemoveLicenseDatabaseUser";
        public static string ActivityRemoveLicenseDatabaseUserRoute = "activity-remove-license-office365-database-user";
        public static Uri RemoveLicenseDatabaseUserUri = new Uri($"{RabbitMqUri}{ActivityRemoveLicenseDatabaseUserRoute}");
        public static string CompensateRemoveLicenseDatabaseUserRoute = $"{ActivityRemoveLicenseDatabaseUserRoute}-{CompensateSuffix}";
        public static Uri CompensateRemoveLicenseDatabaseUserUri = new Uri($"{RabbitMqUri}{CompensateRemoveLicenseDatabaseUserRoute}");

        public static string ActivityRemoveLicensePartnerPortalUser = "ActivityRemoveLicensePartnerPortalUser";
        public static string ActivityRemoveLicensePartnerPortalUserRoute = "activity-remove-license-office365-partner-portal-user";
        public static Uri RemoveLicensePartnerPortalUserUri = new Uri($"{RabbitMqUri}{ActivityRemoveLicensePartnerPortalUserRoute}");
        public static string CompensateRemoveLicensePartnerPortalUserRoute = $"{ActivityRemoveLicensePartnerPortalUserRoute}-{CompensateSuffix}";
        public static Uri CompensateRemoveLicensePartnerPortalUserUri = new Uri($"{RabbitMqUri}{CompensateRemoveLicensePartnerPortalUserRoute}");
        
        public static string ActivityRemoveAllLicensesPartnerPortalUser = "ActivityRemoveAllLicensesPartnerPortalUser";
        public static string ActivityRemoveAllLicensesPartnerPortalUserRoute = "activity-office365-remove-all-licenses-partner-portal-user";
        public static Uri ActivityRemoveAllLicensesPartnerPortalUserUri = new Uri($"{RabbitMqUri}{ActivityRemoveAllLicensesPartnerPortalUserRoute}");

        public static string ActivityGetUserRoles = "ActivityGetUserRoles";
        public static string ActivityGetUserRolesRoute = "activity-get-office365-user-roles";
        public static Uri ActivityGetUserRolesUri = new Uri($"{RabbitMqUri}{ActivityGetUserRolesRoute}");

        public static string ActivityAssignUserRoles = "ActivityAssignUserRoles";
        public static string ActivityAssignUserRolesRoute = "activity-assign-office365-user-roles";
        public static Uri AssignUserRolesUri = new Uri($"{RabbitMqUri}{ActivityAssignUserRolesRoute}");

        public static string ActivityRemoveUserRoles = "ActivityRemoveUserRoles";
        public static string ActivityRemoveUserRolesRoute = "activity-remove-roles-office365-user";
        public static Uri RemoveUserRolesUri = new Uri($"{RabbitMqUri}{ActivityRemoveUserRolesRoute}");
        public static string CompensateRemoveUserRolesRoute = $"{ActivityRemoveUserRolesRoute}-{CompensateSuffix}";
        public static Uri CompensateRemoveUserRolesUri = new Uri($"{RabbitMqUri}{CompensateRemoveUserRolesRoute}");

        public static string ActivitySendOffice365UserSetupEmail = "ActivitySendOffice365UserSetup";
        public static string ActivitySendOffice365UserSetupEmailRoute = "activity-send-office365-user-setup-email";
        public static Uri SendOffice365UserSetupEmailUri = new Uri($"{RabbitMqUri}{ActivitySendOffice365UserSetupEmailRoute}");

        // Office 365 Domain
        public static string QueueOffice365DomainVerification = "office-365-domain-verification-flow";
        public static string QueueAddAdditionalOffice365Domain = "add-additional-office365-domain-flow";
        public static string QueueOffice365ResendTxtRecords = "office-365-resend-txt-records-flow";
        public static string QueueManageSubscriptionsAndLicences = "office-365-manage-subscriptions-and-licences";

        public static Uri QueueManageSubscriptionsAndLicencesUri = new Uri($"{RabbitMqUri}{QueueManageSubscriptionsAndLicences}");


        public static string ActivityAddOffice365CustomerDomainToPartnerPortal = "ActivityAddOffice365CustomerDomainToPartnerPortal";
        public static string ActivityAddOffice365CustomerDomainToPartnerPortalRoute = "activity-add-office365-customer-domain-to-partner-poratal";
        public static Uri AddOffice365CustomerDomainToPartnerPortalUri = new Uri($"{RabbitMqUri}{ActivityAddOffice365CustomerDomainToPartnerPortalRoute}");
        public static string CompensateAddOffice365CustomerDomainRoute = $"{ActivityAddOffice365CustomerDomainToPartnerPortalRoute}-{CompensateSuffix}";
        public static Uri CompensateAddOffice365CustomerDomainUri = new Uri($"{RabbitMqUri}{CompensateAddOffice365CustomerDomainRoute}");

        public static string ActivityAddOffice365CustomerDomainToDatabase = "ActivityAddOffice365CustomerDomainToDatabase";
        public static string ActivityAddOffice365CustomerDomainToDatabaseRoute = "activity-add-office365-customer-domain-to-database";
        public static Uri AddOffice365CustomerDomainToDatabaseUri = new Uri($"{RabbitMqUri}{ActivityAddOffice365CustomerDomainToDatabaseRoute}");

        public static string ActivityAddMultiDomainToDatabase = "ActivityAddMultiDomainToDatabase";
        public static string ActivityAddMultiDomainToDatabaseRoute = "activity-add-office365-multi-domain-to-database";
        public static Uri AddMultiDomainToDatabaseUri = new Uri($"{RabbitMqUri}{ActivityAddMultiDomainToDatabaseRoute}");
        public static string CompensateAddMultiDomainToDatabaseRoute = $"{ActivityAddMultiDomainToDatabaseRoute}-{CompensateSuffix}";
        public static Uri CompensateAddMultiDomainToDatabaseUri = new Uri($"{RabbitMqUri}{CompensateAddMultiDomainToDatabaseRoute}");

        public static string ActivityGetOffice365CustomerTxtRecords = "ActivityGetOffice365CustomerTxtRecords";
        public static string ActivityGetOffice365CustomerTxtRecordsRoute = "activity-get-o365-txt-records";
        public static Uri GetOffice365CustomerTxtRecordsUri = new Uri($"{RabbitMqUri}{ActivityGetOffice365CustomerTxtRecordsRoute}");

        public static string ActivitySendOffice365CustomerTxtRecords = "ActivitySendOffice365CustomerTxtRecords";
        public static string ActivitySendOffice365CustomerTxtRecordsRoute = "activity-send-office365-txt-records";
        public static Uri SendOffice365CustomerTxtRecordsUri = new Uri($"{RabbitMqUri}{ActivitySendOffice365CustomerTxtRecordsRoute}");
        
        public static string ActivityVerifyCustomerDomain = "ActivityVerifyCustomerDomain";
        public static string ActivityVerifyCustomerDomainRoute = "activity-verify-office365-domain";
        public static Uri VerifyCustomerDomainUri = new Uri($"{RabbitMqUri}{ActivityVerifyCustomerDomainRoute}");

        public static string ActivityVerifyCustomerDomainDatabaseStatus = "ActivityVerifyCustomerDomainDatabaseStatus";
        public static string ActivityVerifyCustomerDomainDatabaseStatusRoute = "activity-verify-customer-domain-database-status";
        public static Uri ActivityVerifyCustomerDomainDatabaseStatusUri = new Uri($"{RabbitMqUri}{ActivityVerifyCustomerDomainDatabaseStatusRoute}");

        public static string ActivityFederateCustomerDomain = "ActivityFederateCustomerDomain";
        public static string ActivityFederateCustomerDomainRoute = "activity-federate-office365-domain";
        public static Uri FederateCustomerDomainUri = new Uri($"{RabbitMqUri}{ActivityFederateCustomerDomainRoute}");

        public static string ActivityFederateCustomerDomainDatabaseStatus = "ActivityFederateCustomerDomainDatabaseStatus";
        public static string ActivityFederateCustomerDomainDatabaseStatusRoute = "activity-federate-office365-domain-database-status";
        public static Uri ActivityFederateCustomerDomainDatabaseStatusRouteUri = new Uri($"{RabbitMqUri}{ActivityFederateCustomerDomainDatabaseStatusRoute}");

        // Office 365 Transition
        public static string QueueOffice365Transition = "office-365-transition-flow";

        public static string QueueOffice365TransitionUserAndLicense = "office-365-transition-user-and-licenses-flow";
        public static Uri QueueOffice365TransitionUserAndLicenseUri => new Uri($"{RabbitMqUri}{QueueOffice365TransitionUserAndLicense}");

        public static string QueueOffice365TransitionDeletePartnerPlatformUser = "office-365-delete-partner-platform-user-flow";
        public static Uri QueueOffice365TransitionDeletePartnerPlatformUserUri => new Uri($"{RabbitMqUri}{QueueOffice365TransitionDeletePartnerPlatformUser}");

        public static string QueueOffice365TransitioReport = "office-365-transition-report-flow";
        public static Uri QueueOffice365TransitioReportUri => new Uri($"{RabbitMqUri}{QueueOffice365TransitioReport}");
        public static string SetImmutableId = "office-365-set-immutableid";

        public static string ActivityTransitionDispatchCreatingUsers = "ActivityTransitionDispatchCreatingUser";
        public static string ActivityTransitionDispatchCreatingUsersRoute = "activity-transition-dispatch-creating-user";
        public static Uri TransitionDispatchCreatingUsersUri = new Uri($"{RabbitMqUri}{ActivityTransitionDispatchCreatingUsersRoute}");

        public static string ActivityDatabaseProvisionedStatusProvisioned = "ActivityDatabaseProvisionedStatusProvisioned";
        public static string ActivityDatabaseProvisionedStatusProvisionedRoute = "activity-database-provisioned-status-provisioned";
        public static Uri ActivityDatabaseProvisionedStatusProvisionedUri = new Uri($"{RabbitMqUri}{ActivityDatabaseProvisionedStatusProvisionedRoute}");
        public static string CompensateDatabaseProvisionedStatusProvisionedRoute = $"{ActivityDatabaseProvisionedStatusProvisionedRoute}-{CompensateSuffix}";
        public static Uri CompensateDatabaseProvisionedStatusProvisionedUri = new Uri($"{RabbitMqUri}{CompensateDatabaseProvisionedStatusProvisionedRoute}");
    }
}
