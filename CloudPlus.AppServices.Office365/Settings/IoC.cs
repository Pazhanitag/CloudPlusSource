using Autofac;
using Autofac.Extras.DynamicProxy;
using CloudPlus.AppServices.Office365.AutofacModules;
using CloudPlus.AppServices.Office365.Consumers.AddressValidation;
using CloudPlus.AppServices.Office365.Consumers.Customer;
using CloudPlus.AppServices.Office365.Consumers.Domain;
using CloudPlus.AppServices.Office365.Consumers.Subscription;
using CloudPlus.AppServices.Office365.Consumers.Transition;
using CloudPlus.AppServices.Office365.Consumers.User;
using CloudPlus.AppServices.Office365.Observers;
using CloudPlus.AppServices.Office365.Workflow;
using CloudPlus.AppServices.Office365.Workflow.AddAdditionalDomain;
using CloudPlus.AppServices.Office365.Workflow.Builder;
using CloudPlus.AppServices.Office365.Workflow.CreateCustomer;
using CloudPlus.AppServices.Office365.Workflow.FederateDomain;
using CloudPlus.AppServices.Office365.Workflow.HardDeleteUser;
using CloudPlus.AppServices.Office365.Workflow.ManageSubscription;
using CloudPlus.AppServices.Office365.Workflow.ResendTxtRecords;
using CloudPlus.AppServices.Office365.Workflow.Transition;
using CloudPlus.AppServices.Office365.Workflow.UserAssignLicense;
using CloudPlus.AppServices.Office365.Workflow.UserChangeRoles;
using CloudPlus.AppServices.Office365.Workflow.UserRemoveLicense;
using CloudPlus.AppServices.Office365.Workflow.UserRestore;
using CloudPlus.AppServices.Office365.Workflow.VerifyDomain;
using CloudPlus.Database;
using CloudPlus.Database.Authentication;
using CloudPlus.DynamicProxy.Interceptors.Loggers;
using CloudPlus.Infrastructure.Http;
using CloudPlus.Infrastructure.ServiceBus.RabbitMq;
using CloudPlus.Resources;
using CloudPlus.Services.Database.Catalog;
using CloudPlus.Services.Database.Company;
using CloudPlus.Services.Database.Office365.Api;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Database.Office365.Domain;
using CloudPlus.Services.Database.Office365.License;
using CloudPlus.Services.Database.Office365.Offer;
using CloudPlus.Services.Database.Office365.Role;
using CloudPlus.Services.Database.Office365.Subscription;
using CloudPlus.Services.Database.Office365.User;
using CloudPlus.Services.Database.Product;
using CloudPlus.Services.Database.ProductItem;
using CloudPlus.Services.Database.Provisions;
using CloudPlus.Services.Database.WorkflowActivity;
using CloudPlus.Services.Database.WorkflowActivity.Office365;
using CloudPlus.Services.Identity.Role;
using CloudPlus.Services.Identity.User;
using CloudPlus.Services.Office365.AddressService;
using CloudPlus.Services.Office365.CustomerService;
using CloudPlus.Services.Office365.Domain;
using CloudPlus.Services.Office365.License;
using CloudPlus.Services.Office365.Operations;
using CloudPlus.Services.Office365.OrderService;
using CloudPlus.Services.Office365.Subscription;
using CloudPlus.Services.Office365.User;
using CloudPlus.Services.Office365.Utilities;
using CloudPlus.Settings;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Common.Consumers;
using CloudPlus.Workflows.Common.Workflow;
using CloudPlus.Workflows.Office365.Activities.Customer.ActivateSuspendedDatabaseSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.ActivateSuspendedPartnerPlatformSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseCustomer;
using CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.CreateOrder;
using CloudPlus.Workflows.Office365.Activities.Customer.CreatePartnerPlatformCustomer;
using CloudPlus.Workflows.Office365.Activities.Customer.DatabaseCustomerSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.DecreaseDatabaseCustomerSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.DecreasePartnerPlatformCustomerSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.MultiDatabaseCustomerSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.MultiPartnerPlatformCustomerSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.PartnerPlatformCustomerSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.SuspendDatabasesubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.SuspendPartnerPlatformSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseCustomerSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscriptionQuantity;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscriptionState;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscription__;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdatePartnerPlatformSubscriptionQuantity;
using CloudPlus.Workflows.Office365.Activities.Domain.AddCustomerDomainPartnerPortalActivity;
using CloudPlus.Workflows.Office365.Activities.Domain.AddCustomerDomainToDatabaseActivity;
using CloudPlus.Workflows.Office365.Activities.Domain.AddMultiDomainToDatabase;
using CloudPlus.Workflows.Office365.Activities.Domain.FederateCustomerDomain;
using CloudPlus.Workflows.Office365.Activities.Domain.FederateCustomerDomainDatabaseStatus;
using CloudPlus.Workflows.Office365.Activities.Domain.GetCustomerTxtRecords;
using CloudPlus.Workflows.Office365.Activities.Domain.SendCustomerTxtRecords;
using CloudPlus.Workflows.Office365.Activities.Domain.VerifyCustomerDomain;
using CloudPlus.Workflows.Office365.Activities.Domain.VerifyCustomerDomainDatabaseStatus;
using CloudPlus.Workflows.Office365.Activities.Transition.DatabaseProvisionedStatusProvisioned;
using CloudPlus.Workflows.Office365.Activities.Transition.TransitionDispatchCreatingUser;
using CloudPlus.Workflows.Office365.Activities.User.ActivateSoftDeletedDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.AssignUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToPartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.CreateDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.CreatePartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.CreateTempPartnerPlatformAdminUser;
using CloudPlus.Workflows.Office365.Activities.User.DeleteDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.HardDeletePartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.SendUserSetupEmail;
using CloudPlus.Workflows.Office365.Mappers;
using CloudPlus.Workflows.Office365.Activities.User.DeletePartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.GetUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.RemoveAllLicensesPartnerPortalUser;
using CloudPlus.Workflows.Office365.Activities.User.RemoveLicenseDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.RemoveLicensePartnerPortalUser;
using CloudPlus.Workflows.Office365.Activities.User.RemoveUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.RestorePartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.SetImmutableId;
using CloudPlus.Workflows.Office365.Activities.User.SoftDeleteDatabaseUser;

namespace CloudPlus.AppServices.Office365.Settings
{
    public static class IoC
    {
        private static IContainer _container;

        internal static IContainer SetupAutofacContainer()
        {
            var builder = new ContainerBuilder();

            RegisterDependencies(builder);

            _container = builder.Build();

            return _container;
        }

        public static IContainer GetContainer()
        {
            return _container;
        }

        private static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterModule<MessageBrokerModule>();

            builder.RegisterType<InterceptorLogger>().As<IInterceptorLogger>();
            builder.RegisterType<ServiceInterceptorLogger>().As<IServiceInterceptorLogger>();
            builder.RegisterType<ConsumerInterceptorLogger>().As<IConsumerInterceptorLogger>();
            builder.RegisterType<ActivityInterceptorLogger>().As<IActivityInterceptorLogger>();

            builder.RegisterType<Office365ProvisioningService>();
            builder.RegisterType<Office365ServiceSettings>().As(typeof(IOffice365ServiceSettings), typeof(IRabbitMqSettings));

            builder.RegisterType<ConfigurationManager>().As<IConfigurationManager>();
            builder.RegisterType<JsonSerializer>().As<IObjectSerializer>();
            builder.RegisterType<HttpClientResolver>().As<IHttpClientResolver>();
            builder.RegisterType<JsonValueSqlBuilder>().As<IJsonValueSqlBuilder>();

            builder.RegisterType<WorkflowActivityConsumer>().As<IWorkflowActivityConsumer>();
            builder.RegisterType<RoutingSlipStartedConsumer>().As<IRoutingSlipStartedConsumer>();
            builder.RegisterType<Office365ManageSubscriptionsAndLicencesObserver>().As<IOffice365ManageSubscriptionsAndLicencesObserver>();
            builder.RegisterType<Office365TransitionDeletePartnerPlatformUserObserver>().As<IOffice365TransitionDeletePartnerPlatformUserObserver>();

            builder.RegisterType<CldpDbContext>().InstancePerDependency();
            builder.RegisterType<CloudPlusAuthDbContext>().InstancePerLifetimeScope();

            builder.RegisterType<Office365TransitionDeletePartnerPlatformUserConsumer>().As<IOffice365TransitionDeletePartnerPlatformUserConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<Office365TransitionUserAndLicensesConsumer>().As<IOffice365TransitionUserAndLicensesConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger)); 
            builder.RegisterType<Office365AddressValidationConsumer>().As<IOffice365AddressValidationConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365CreateCustomerConsumer>().As<IOffice365CreateCustomerConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365UserAssignLicenseConsumer>().As<IOffice365UserAssignLicenseConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365UserChangeLicenseConsumer>().As<IOffice365UserChangeLicenseConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365UserRemoveLicenseConsumer>().As<IOffice365UserRemoveLicenseConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365UserRestoreConsumer>().As<IOffice365UserRestoreConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365UserMultiEditConsumer>().As<IOffice365UserMultiEditConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365VerifyCustomerDomainConsumer>().As<IOffice365VerifyCustomerDomainConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365GetUserRolesConsumer>().As<IOffice365GetUserRolesConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365UserChangeRolesConsumer>().As<IOffice365UserChangeRolesConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365AddAdditionalDomainConsumer>().As<IOffice365AddAdditionalDomainConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365ResendTxtRecordsConsumer>().As<IOffice365ResendTxtRecordsConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365TransitionConsumer>().As<IOffice365TransitionConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365TransitionReportConsumer>().As<IOffice365TransitionReportConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger)); 
            builder.RegisterType<Office365FederateDomainConsumer>().As<IOffice365FederateDomainConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365HardDeleteUserConsumer>().As<IOffice365HardDeleteUserConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<Office365CreateUserConsumer>().As<IOffice365CreateUserConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            
            builder.RegisterType<ManageSubscriptionsAndLicencesConsumer>().As<IManageSubscriptionsAndLicencesConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<SuspendPartnerPlatformSubscriptionActivity>().As<ISuspendPartnerPlatformSubscriptionActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<SuspendDatabasesubscriptionActivity>().As<ISuspendDatabasesubscriptionActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<ActivateSuspendedDatabaseSubscriptionActivity>().As<IActivateSuspendedDatabaseSubscriptionActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<ActivateSuspendedPartnerPlatformSubscriptionActivity>().As<IActivateSuspendedPartnerPlatformSubscriptionAcivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            
            builder.RegisterType<RabbitMqMessageBroker>().As<IRabbitMqMessageBroker>();

            builder.RegisterType<WorkflowActivityService>().As<IWorkflowActivityService>();

            builder.RegisterType<Office365WorkflowBuilder>().As<IWorkflowBuilder>();
            builder.RegisterType<ActivityConfigurator>().As<IActivityConfigurator>();

            builder.RegisterType<Office365CreateCustomerWorkflow>().As<IOffice365CreateCustomerWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<Office365UserAssignLicenseWorkflow>().As<IOffice365UserAssignLicenseWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<Office365UserRestoreWorkflow>().As<IOffice365UserRestoreWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<Office365UserRemoveLicenseWorkflow>().As<IOffice365UserRemoveLicenseWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<Office365UserChangeRolesWorkflow>().As<IOffice365UserChangeRolesWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<Office365ResendTxtRecordsWorkflow>().As<IOffice365ResendTxtRecordsWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<Office365AddAdditionalDomainWorkflow>().As<IOffice365AddAdditionalDomainWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<Office365VerifyDomainWorkflow>().As<IOffice365VerifyDomainWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<Office365TransitionWorkflow>().As<IOffice365TransitionWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<MultiUserEditWorkflow>().As<IMultiUserEditWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<ManageSubscriptionWorkflow>().As<IManageSubscriptionWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<FederateDomainWorkflow>().As<IFederateDomainWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<HardDeleteUserWorkflow>().As<IHardDeleteUserWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            

            builder.RegisterType<Office365TransitionUserAndLicensesWorkflow>().As<IOffice365TransitionUserAndLicensesWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger)); 
            builder.RegisterType<PartnerOperations>().As<IPartnerOperations>();

            builder.RegisterType<AddressValidationService>().As<IAddressValidationService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365CustomerService>().As<IOffice365CustomerService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365DbCustomerService>().As<IOffice365DbCustomerService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365DomainService>().As<IOffice365DomainService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365DbDomainService>().As<IOffice365DbDomainService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365DbSubscriptionService>().As<IOffice365DbSubscriptionService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office356DbOfferService>().As<IOffice356DbOfferService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365ApiService>().As<IOffice365ApiService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365UserService>().As<IOffice365UserService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365DbUserService>().As<IOffice365DbUserService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CompanyService>().As<ICompanyService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CompanyCatalogService>().As<ICompanyCatalogService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CatalogProductItemService>().As<ICatalogProductItemService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CatalogUtilities>().As<ICatalogUtilities>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CustomerCatalogService>().As<ICustomerCatalogService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<UserService>().As<IUserService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<RoleService>().As<IRoleService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365OrderService>().As<IOffice365OrderService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365SubscriptionService>().As<IOffice365SubscriptionService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365LicenseService>().As<IOffice365LicenseService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365DbLicenseService>().As<IOffice365DbLicenseService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365UtilitiesService>().As<IOffice365UtilitiesService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<Office365DbRoleService>().As<IOffice365DbRoleService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger)); 
            builder.RegisterType<ProvisioningService>().As<IProvisioningService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<WorkflowOffice365ActivityService>().As<IWorkflowOffice365ActivityService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<ProductService>().As<IProductService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<ProductItemService>().As<IProductItemService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));




            builder.RegisterType<ActivityOffice365CustomerArgumentsMapper>().As<IActivityOffice365CustomerArgumentsMapper>();
            builder.RegisterType<ActivityOffice365AddAdditionalDomainArgumentsMapper>().As<IActivityOffice365AddAdditionalDomainArgumentsMapper>();
            builder.RegisterType<ActivityOffice365UserArgumentsMapper>().As<IActivityOffice365UserArgumentsMapper>();
            builder.RegisterType<ActivityOffice365UserRestoreMapper>().As<IActivityOffice365UserRestoreMapper>();
            builder.RegisterType<ActivityOffice365UserChangeRolesMapper>().As<IActivityOffice365UserChangeRolesMapper>();
            builder.RegisterType<ActivityOffice365TransitionArgumentsMapper>().As<IActivityOffice365TransitionArgumentsMapper>();
            builder.RegisterType<ActivityOffice365TransitionUserAndLicensesArgumentsMapper>().As<IActivityOffice365TransitionUserAndLicensesArgumentsMapper>();

            builder.RegisterType<CreatePartnerPlatformCustomerActivity>().As<ICreatePartnerPlatformCustomerActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<CreateDatabaseCustomerActivity>().As<ICreateDatabaseCustomerActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<AddCustomerDomainPartnerPortalActivity>().As<IAddCustomerDomainPartnerPortalActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<AddCustomerDomainToDatabaseActivity>().As<IAddCustomerDomainToDatabaseActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<AddMultiDomainToDatabaseActivity>().As<IAddMultiDomainToDatabaseActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<GetCustomerDomainTxtRecordsActivity>().As<IGetCustomerDomainTxtRecordsActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<SendCustomerDomainTxtRecordsActivity>().As<ISendCustomerDomainTxtRecordsActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<PartnerPlatformCustomerSubscriptionActivity>().As<IPartnerPlatformCustomerSubscriptionActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<MultiPartnerPlatformCustomerSubscriptionActivity>().As<IMultiPartnerPlatformCustomerSubscriptionActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<DecreasePartnerPlatformCustomerSubscriptionActivity>().As<IDecreasePartnerPlatformCustomerSubscriptionActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<DatabaseCustomerSubscriptionActivity>().As<IDatabaseCustomerSubscriptionActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<MultiDatabaseCustomerSubscriptionActivity>().As<IMultiDatabaseCustomerSubscriptionActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<UpdateDatabaseCustomerSubscriptionActivity>().As<IUpdateDatabaseCustomerSubscriptionActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<DecreaseDatabaseCustomerSubscriptionActivity>().As<IDecreaseDatabaseCustomerSubscriptionActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));

            builder.RegisterType<CreatePartnerPlatformUserActivity>().As<ICreatePartnerPlatformUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<CreateDatabaseUserActivity>().As<ICreateDatabaseUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<AssignLicenseToPartnerPlatformUserActivity>().As<IAssignLicenseToPartnerPlatformUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<AssignLicenseToDatabaseUserActivity>().As<IAssignLicenseToDatabaseUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<RemoveLicenseDatabaseUserActivity>().As<IRemoveLicenseDatabaseUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<RemoveLicensePartnerPortalUserActivity>().As<IRemoveLicensePartnerPortalUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<RemoveAllLicensesPartnerPortalUserActivity>().As<IRemoveAllLicensesPartnerPortalUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<SendUserSetupEmailActivity>().As<ISendUserSetupEmailActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));

            builder.RegisterType<VerifyCustomerDomainActivity>().As<IVerifyCustomerDomainActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<VerifyCustomerDomainDatabaseStatusActivity>().As<IVerifyCustomerDomainDatabaseStatusActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<CreateTempPartnerPlatformAdminUserActivity>().As<ICreateTempPartnerPlatformAdminUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<GetUserRolesActivity>().As<IGetUserRolesActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<AssignUserRolesActivity>().As<IAssignUserRolesActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<RemoveUserRolesActivity>().As<IRemoveUserRolesActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<HardDeletePartnerPlatformUserActivity>().As<IHardDeletePartnerPlatformUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<DeletePartnerPlatformUserActivity>().As<IDeletePartnerPlatformUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<DeleteDatabaseUserActivity>().As<IDeleteDatabaseUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<SoftDeleteDatabaseUserActivity>().As<ISoftDeleteDatabaseUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<RestorePartnerPlatformUserActivity>().As<IRestorePartnerPlatformUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<ActivateSoftDeletedDatabaseUserActivity>().As<IActivateSoftDeletedDatabaseUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<FederateCustomerDomainActivity>().As<IFederateCustomerDomainActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<FederateCustomerDomainDatabaseStatusActivity>().As<IFederateCustomerDomainDatabaseStatusActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<TransitionDispatchCreatingUsersActivity>().As<ITransitionDispatchCreatingUsersActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));

            builder.RegisterType<CreateDatabaseSubscriptionActivity>().As<ICreateDatabaseSubscriptionActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<CreateOrderActivity>().As<ICreateOrderActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<UpdateDatabaseSubscriptionActivity>().As<IUpdateDatabaseSubscriptionActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<UpdateDatabaseSubscriptionStateActivity>().As<IUpdateDatabaseSubscriptionStateActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<UpdateDatabaseSubscriptionQuantityActivity>().As<IUpdateDatabaseSubscriptionQuantityActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<UpdatePartnerPlatformSubscriptionQuantityActivity>().As<IUpdatePartnerPlatformSubscriptionQuantityActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));

            builder.RegisterType<SetImmutableIdActivity>().As<ISetImmutableIdActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            
            builder.RegisterType<DatabaseProvisionedStatusProvisionedActivity>().As<IDatabaseProvisionedStatusProvisionedActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger)); 
        }
    }
}