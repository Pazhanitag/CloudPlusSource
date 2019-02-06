using System;
using Autofac.Core;
using Autofac.Core.Lifetime;
using MassTransit.RabbitMqTransport;
using CloudPlus.Workflows.Common.ActivityConfigurator;
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
using CloudPlus.Workflows.Office365.Activities.Domain.GetCustomerTxtRecords;
using CloudPlus.Workflows.Office365.Activities.Domain.SendCustomerTxtRecords;
using CloudPlus.Workflows.Office365.Activities.Domain.AddCustomerDomainToDatabaseActivity;
using CloudPlus.Workflows.Office365.Activities.Domain.AddMultiDomainToDatabase;
using CloudPlus.Workflows.Office365.Activities.Domain.FederateCustomerDomain;
using CloudPlus.Workflows.Office365.Activities.Domain.FederateCustomerDomainDatabaseStatus;
using CloudPlus.Workflows.Office365.Activities.Domain.VerifyCustomerDomain;
using CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToPartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.Domain.VerifyCustomerDomainDatabaseStatus;
using CloudPlus.Workflows.Office365.Activities.Transition.DatabaseProvisionedStatusProvisioned;
using CloudPlus.Workflows.Office365.Activities.Transition.TransitionDispatchCreatingUser;
using CloudPlus.Workflows.Office365.Activities.User.ActivateSoftDeletedDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.AssignUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.CreateDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.CreatePartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.CreateTempPartnerPlatformAdminUser;
using CloudPlus.Workflows.Office365.Activities.User.DeleteDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.DeletePartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.GetUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.HardDeletePartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.RemoveAllLicensesPartnerPortalUser;
using CloudPlus.Workflows.Office365.Activities.User.RemoveLicenseDatabaseUser;
using CloudPlus.Workflows.Office365.Activities.User.RemoveLicensePartnerPortalUser;
using CloudPlus.Workflows.Office365.Activities.User.RemoveUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.RestorePartnerPlatformUser;
using CloudPlus.Workflows.Office365.Activities.User.SendUserSetupEmail;
using CloudPlus.Workflows.Office365.Activities.User.SetImmutableId;
using CloudPlus.Workflows.Office365.Activities.User.SoftDeleteDatabaseUser;
using GreenPipes;
using System.Collections.Generic;
using CloudPlus.Workflows.Office365.Activities.Customer.CreateOrderWithMultiItems;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscriptionState;
using CloudPlus.Workflows.Office365.Activities.Customer.ActivateMultiSuspendedDatabaseSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.ActivateMultiSuspendedPartnerPlatformSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscriptionQuantity;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiPartnerPlatformSubscriptionQuantity;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateSecurityGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseSecurityGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseDistributionGroup;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseDistributionGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseO365Group;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseO365GroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseSecurityGroup;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDistriputionGroup;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDistriputionGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateO365Group;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateO365GroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateSecurityGroup;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDatabaseDistributionGroup;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDatabaseDistributionGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDatabaseO365Group;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDatabaseO365GroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDatabaseSecurityGroup;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDatabaseSecurityGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDistriputionGroup;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDistriputionGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveO365Group;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveO365GroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveSecurityGroup;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveSecurityGroupMember;


namespace CloudPlus.AppServices.Office365.Workflow.Builder
{
    public class Office365WorkflowBuilder : IWorkflowBuilder
    {
        private readonly IActivityConfigurator _activityConfigurator;

        public Office365WorkflowBuilder(IActivityConfigurator activityConfigurator)
        {
            _activityConfigurator = activityConfigurator;
        }

        public void BuildWorkflow(IRabbitMqBusFactoryConfigurator busFactoryConfigurator, IRabbitMqHost host,
            IComponentRegistry componentRegistry)
        {
            _activityConfigurator
                .ConfigureActivity<IRemoveUserRolesActivity, IRemoveUserRolesArguments, IRemoveUserRolesLog>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry),
                    executeConfigurator => { executeConfigurator.UseConcurrencyLimit(1); });
            _activityConfigurator
                .ConfigureActivity<IDeletePartnerPlatformUserActivity, IDeletePartnerPlatformUserArguments,
                    IDeletePartnerPlatformUserLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<ISoftDeleteDatabaseUserActivity, ISoftDeleteDatabaseUserArguments,
                    ISoftDeleteDatabaseUserLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IRestorePartnerPlatformUserActivity, IRestorePartnerPlatformUserArguments,
                    IRestorePartnerPlatformUserLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IActivateSoftDeletedDatabaseUserActivity, IActivateSoftDeletedDatabaseUserArguments,
                    IActivateSoftDeletedDatabaseUserLog>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<ICreatePartnerPlatformCustomerActivity, ICreatePartnerPlatformCustomerArguments,
                    ICreatePartnerPlatformCustomerLog>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<ICreateDatabaseCustomerActivity, ICreateDatabaseCustomerArguments,
                    ICreateDatabaseCustomerLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IAddCustomerDomainPartnerPortalActivity, IAddCustomerDomainPartnerPortalArguments,
                    IAddCustomerDomainPartnerPortalLog>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry),
                    executeConfigurator => { executeConfigurator.UseConcurrencyLimit(1); });
            _activityConfigurator
                .ConfigureActivity<IPartnerPlatformCustomerSubscriptionActivity,
                    IPartnerPlatformCustomerSubscriptionArguments, IPartnerPlatformCustomerSubscriptionLog>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IDecreasePartnerPlatformCustomerSubscriptionActivity,
                    IDecreasePartnerPlatformCustomerSubscriptionArguments,
                    IDecreasePartnerPlatformCustomerSubscriptionLog>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IDatabaseCustomerSubscriptionActivity, IDatabaseCustomerSubscriptionArguments,
                    IDatabaseCustomerSubscriptionLog>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry),
                    executeConfigurator =>
                    {
                        executeConfigurator.UseRetry(r => { r.Interval(30, TimeSpan.FromSeconds(10)); });
                    });
            _activityConfigurator
                .ConfigureActivity<IDecreaseDatabaseCustomerSubscriptionActivity,
                    IDecreaseDatabaseCustomerSubscriptionArguments, IDecreaseDatabaseCustomerSubscriptionLog>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<ICreatePartnerPlatformUserActivity, ICreatePartnerPlatformUserArguments,
                    ICreatePartnerPlatformUserLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry),
                    executeConfigurator => { executeConfigurator.UseConcurrencyLimit(1); });
            _activityConfigurator
                .ConfigureActivity<ICreateDatabaseUserActivity, ICreateDatabaseUserArguments, ICreateDatabaseUserLog>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IRemoveLicensePartnerPortalUserActivity, IRemoveLicensePartnerPortalUserArguments,
                    IRemoveLicensePartnerPortalUserLog>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IRemoveLicenseDatabaseUserActivity, IRemoveLicenseDatabaseUserArguments,
                    IRemoveLicenseDatabaseUserLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IMultiPartnerPlatformCustomerSubscriptionActivity,
                    IMultiPartnerPlatformCustomerSubscriptionArguments, IMultiPartnerPlatformCustomerSubscriptionLog>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IMultiDatabaseCustomerSubscriptionActivity,
                    IMultiDatabaseCustomerSubscriptionArguments, IMultiDatabaseCustomerSubscriptionLog>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IAddMultiDomainToDatabaseActivity, IAddMultiDomainToDatabaseArguments,
                    IAddMultiDomainToDatabaseLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<ICreateDatabaseSubscriptionActivity, ICreateDatabaseSubscriptionArguments,
                    ICreateDatabaseSubscriptionLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator.ConfigureActivity<ICreateOrderActivity, ICreateOrderArguments, ICreateOrderLog>(
                busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IUpdateDatabaseSubscriptionActivity, IUpdateDatabaseSubscriptionArguments,
                    IUpdateDatabaseSubscriptionLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IUpdateDatabaseSubscriptionQuantityActivity,
                    IUpdateDatabaseSubscriptionQuantityArguments, IUpdateDatabaseSubscriptionQuantityLog>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IUpdatePartnerPlatformSubscriptionQuantityActivity,
                    IUpdatePartnerPlatformSubscriptionQuantityArguments, IUpdatePartnerPlatformSubscriptionQuantityLog>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<ISuspendDatabasesubscriptionActivity, ISuspendDatabasesubscriptionArguments,
                    ISuspendDatabasesubscriptionLog>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<ISuspendPartnerPlatformSubscriptionActivity,
                    ISuspendPartnerPlatformSubscriptionArguments, ISuspendPartnerPlatformSubscriptionLog>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IActivateSuspendedDatabaseSubscriptionActivity,
                    IActivateSuspendedDatabaseSubscriptionArguments, IActivateSuspendedDatabaseSubscriptionLog>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IActivateSuspendedPartnerPlatformSubscriptionAcivity,
                    IActivateSuspendedPartnerPlatformSubscriptionArguments,
                    IActivateSuspendedPartnerPlatformSubscriptionLog>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureActivity<IDatabaseProvisionedStatusProvisionedActivity,
                    IDatabaseProvisionedStatusProvisionedArguments, IDatabaseProvisionedStatusProvisionedLog>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureExecuteActivity<IUpdateDatabaseSubscriptionStateActivity,
                    IUpdateDatabaseSubscriptionStateArguments>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry));
            _activityConfigurator.ConfigureExecuteActivity<IGetUserRolesActivity, IGetUserRolesArguments>(
                busFactoryConfigurator, host, new LifetimeScope(componentRegistry),
                executeConfigurator => { executeConfigurator.UseConcurrencyLimit(1); });
            _activityConfigurator.ConfigureExecuteActivity<IAssignUserRolesActivity, IAssignUserRolesArguments>(
                busFactoryConfigurator, host, new LifetimeScope(componentRegistry),
                executeConfigurator => { executeConfigurator.UseConcurrencyLimit(1); });
            _activityConfigurator
                .ConfigureExecuteActivity<IFederateCustomerDomainActivity, IFederateCustomerDomainArguments>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry),
                    executeConfigurator => { executeConfigurator.UseConcurrencyLimit(1); });
            _activityConfigurator
                .ConfigureExecuteActivity<IFederateCustomerDomainDatabaseStatusActivity,
                    IFederateCustomerDomainDatabaseStatusArguments>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureExecuteActivity<IHardDeletePartnerPlatformUserActivity,
                    IHardDeletePartnerPlatformUserArguments>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry),
                    executeConfigurator => { executeConfigurator.UseConcurrencyLimit(1); });
            _activityConfigurator.ConfigureExecuteActivity<IDeleteDatabaseUserActivity, IDeleteDatabaseUserArguments>(
                busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureExecuteActivity<ICreateTempPartnerPlatformAdminUserActivity,
                    ICreateTempPartnerPlatformAdminUserArguments>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry),
                    executeConfigurator =>
                    {
                        executeConfigurator.UseConcurrencyLimit(1);

                    });

            _activityConfigurator
                .ConfigureExecuteActivity<IVerifyCustomerDomainActivity, IVerifyCustomerDomainArguments>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry),
                    executeConfigurator =>
                    {
                        executeConfigurator.UseConcurrencyLimit(1);

                    });

            _activityConfigurator.ConfigureExecuteActivity<ISetImmutableIdActivity, SetImmutableIdArguments>(
                busFactoryConfigurator, host, new LifetimeScope(componentRegistry),
                executeConfigurator =>
                {
                    executeConfigurator.UseConcurrencyLimit(1);
                });

            _activityConfigurator
                .ConfigureExecuteActivity<IVerifyCustomerDomainDatabaseStatusActivity,
                    IVerifyCustomerDomainDatabaseStatusArguments>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureExecuteActivity<IAddCustomerDomainToDatabaseActivity, IAddCustomerDomainToDatabaseArguments>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureExecuteActivity<IGetCustomerDomainTxtRecordsActivity, IGetCustomerDomainTxtRecordsArguments>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry),
                    executeConfigurator => { executeConfigurator.UseConcurrencyLimit(1); });
            _activityConfigurator
                .ConfigureExecuteActivity<ISendCustomerDomainTxtRecordsActivity, ISendCustomerDomainTxtRecordsArguments
                >(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureExecuteActivity<IUpdateDatabaseCustomerSubscriptionActivity,
                    IUpdateDatabaseCustomerSubscriptionArguments>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureExecuteActivity<IAssignLicenseToPartnerPlatformUserActivity,
                    IAssignLicenseToPartnerPlatformUserArguments>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureExecuteActivity<IAssignLicenseToDatabaseUserActivity, IAssignLicenseToDatabaseUserArguments>(
                    busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator.ConfigureExecuteActivity<ISendUserSetupEmailActivity, ISendUserSetupEmailArguments>(
                busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureExecuteActivity<IRemoveAllLicensesPartnerPortalUserActivity,
                    IRemoveAllLicensesPartnerPortalUserArguments>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry));
            _activityConfigurator
                .ConfigureExecuteActivity<ITransitionDispatchCreatingUsersActivity,
                    ITransitionDispatchCreatingUsersArguments>(busFactoryConfigurator, host,
                    new LifetimeScope(componentRegistry));
            _activityConfigurator
               .ConfigureExecuteActivity<ICreateOrderWithMultiItemsActivity,
                   ICreateOrderWithMultiItemsArguments>(busFactoryConfigurator, host,
                   new LifetimeScope(componentRegistry));
            _activityConfigurator
              .ConfigureExecuteActivity<IUpdateMultiDatabaseSubscriptionActivity,
                  IUpdateMultiDatabaseSubscriptionArguments>(busFactoryConfigurator, host,
                  new LifetimeScope(componentRegistry));
            _activityConfigurator
              .ConfigureExecuteActivity<IUpdateMultiDatabaseSubscriptionStateActivity,
                  IUpdateMultiDatabaseSubscriptionStateArguments>(busFactoryConfigurator, host,
                  new LifetimeScope(componentRegistry));
            _activityConfigurator
  .ConfigureExecuteActivity<IActivateMultiSuspendedDatabaseSubscriptionActivity,
      IActivateMultiSuspendedDatabaseSubscriptionArguments>(busFactoryConfigurator, host,
      new LifetimeScope(componentRegistry));
            _activityConfigurator
  .ConfigureExecuteActivity<IActivateMultiSuspendedPartnerPlatformSubscriptionActivity,
      IActivateMultiSuspendedPartnerPlatformSubscriptionArguments>(busFactoryConfigurator, host,
      new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<IUpdateMultiDatabaseSubscriptionQuantityActivity,
IUpdateMultiDatabaseSubscriptionQuantityArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<IUpdateMultiPartnerPlatformSubscriptionQuantityActivity,
IUpdateMultiPartnerPlatformSubscriptionQuantityArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<ICreateSecurityGroupMemberActivity,
ICreateSecurityGroupMemberArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<ICreateDatabaseSecurityGroupMemberActivity,
ICreateDatabaseSecurityGroupMemberArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));

            _activityConfigurator
.ConfigureExecuteActivity<ICreateDatabaseDistributionGroupActivity,
ICreateDatabaseDistributionGroupArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<ICreateDatabaseDistributionGroupMemberActivity,
ICreateDatabaseDistributionGroupMemberArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<ICreateDatabaseO365GroupActivity,
ICreateDatabaseO365GroupArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<ICreateDatabaseO365GroupMemberActivity,
ICreateDatabaseO365GroupMemberArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<ICreateDatabaseSecurityGroupActivity,
ICreateDatabaseSecurityGroupArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<ICreateDistributionGroupActivity,
ICreateDistributionGroupArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<ICreateDistributionGroupMemberActivity,
ICreateDistributionGroupMemberArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<ICreateO365GroupActivity,
ICreateO365GroupArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<ICreateO365GroupMemberActivity,
ICreateO365GroupMemberArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<ICreateSecurityGroupActivity,
ICreateSecurityGroupArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));

            _activityConfigurator
.ConfigureExecuteActivity<RemoveDatabaseDistributionGroupActivity,
IRemoveDatabaseDistributionGroupArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<IRemoveDatabaseDistributionGroupMemberActivity,
IRemoveDatabaseDistributionGroupMemberArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<IRemoveDatabaseO365GroupActivity,
IRemoveDatabaseO365GroupArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<IRemoveDatabaseO365GroupMemberActivity,
IRemoveDatabaseO365GroupMemberArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<IRemoveDatabaseSecurityGroupActivity,
IRemoveDatabaseSecurityGroupArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<IRemoveDistriputionGroupMemberActivity,
IRemoveDistriputionGroupMemberArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<IRemoveO365GroupActivity,
IRemoveO365GroupArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<IRemoveO365GroupMemberActivity,
IRemoveO365GroupMemberArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<IRemoveSecurityGroupActivity,
IRemoveSecurityGroupArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<IRemoveSecurityGroupMemberActivity,
IRemoveSecurityGroupMemberArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<IRemoveDatabaseSecurityGroupMemberActivity,
IRemoveDatabaseSecurityGroupMemberArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));
            _activityConfigurator
.ConfigureExecuteActivity<IRemoveDistriputionGroupActivity,
IRemoveDistriputionGroupArguments>(busFactoryConfigurator, host,
new LifetimeScope(componentRegistry));

        }
    }
}