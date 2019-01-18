using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using CloudPlus.Constants;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Common;
using CloudPlus.QueueModels.Office365.Domain.Commands;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Office365.Activities.Domain.FederateCustomerDomain;
using CloudPlus.Workflows.Office365.Activities.Domain.FederateCustomerDomainDatabaseStatus;
using CloudPlus.Workflows.Office365.Activities.Domain.VerifyCustomerDomain;
using CloudPlus.Workflows.Office365.Activities.Domain.VerifyCustomerDomainDatabaseStatus;
using CloudPlus.Workflows.Office365.Activities.User.AssignUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.CreateTempPartnerPlatformAdminUser;
using CloudPlus.Workflows.Office365.Activities.User.HardDeletePartnerPlatformUser;
using CloudPlus.Workflows.Office365.Mappers;

namespace CloudPlus.AppServices.Office365.Workflow.VerifyDomain
{
    public class Office365VerifyDomainWorkflow : IOffice365VerifyDomainWorkflow
    {
        private readonly IActivityOffice365CustomerArgumentsMapper _activityOffice365CustomerArgumentsMapper;
        private readonly IActivityConfigurator _activityConfigurator;

        public Office365VerifyDomainWorkflow(IActivityOffice365CustomerArgumentsMapper activityOffice365CustomerArgumentsMapper, IActivityConfigurator activityConfigurator)
        {
            _activityOffice365CustomerArgumentsMapper = activityOffice365CustomerArgumentsMapper;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<IOffice365VerifyDomainCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var command = context.Message;

            builder.AddActivity(Office365ServiceConstants.ActivityVerifyCustomerDomain, 
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IVerifyCustomerDomainActivity)),
                _activityOffice365CustomerArgumentsMapper.MapVerifyCustomerDomain(command));

            builder.AddActivity(Office365ServiceConstants.ActivityCreateTempAdminUser, 
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateTempPartnerPlatformAdminUserActivity)),
                _activityOffice365CustomerArgumentsMapper.MapCreateTempAdminUser(command));

            builder.AddActivity(Office365ServiceConstants.ActivityAssignUserRoles, 
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignUserRolesActivity)),
                _activityOffice365CustomerArgumentsMapper.MapAssignUserRoles(command));

            builder.AddActivity(Office365ServiceConstants.ActivityFederateCustomerDomain, 
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IFederateCustomerDomainActivity)),
                _activityOffice365CustomerArgumentsMapper.MapFederateCustomerDomain(command));

            builder.AddActivity(Office365ServiceConstants.ActivityVerifyCustomerDomainDatabaseStatus,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IVerifyCustomerDomainDatabaseStatusActivity)),
                _activityOffice365CustomerArgumentsMapper.MapVerifyCustomerDomainDatabaseStatus(command));

            builder.AddActivity(Office365ServiceConstants.ActivityFederateCustomerDomainDatabaseStatus,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IFederateCustomerDomainDatabaseStatusActivity)),
                _activityOffice365CustomerArgumentsMapper.MapFederateCustomerDomainDatabaseStatus(command));

            builder.AddActivity(Office365ServiceConstants.ActivityHardDeletePartnerPlatformUser, 
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IHardDeletePartnerPlatformUserActivity)),
                _activityOffice365CustomerArgumentsMapper.MapDeleteTempAdminUser(command));

            builder.AddVariable("domainName", context.Message.DomainName);
            builder.AddVariable("workflowActivityType", WorkflowActivityType.VerifyAndFederateOffice365Domain.ToString());

            builder.AddSubscription(Office365ServiceConstants.QueueOffice365RoutingSlipEventUri,
                RoutingSlipEvents.Faulted |
                RoutingSlipEvents.Completed |
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
                WorkflowActivityType = WorkflowActivityType.VerifyAndFederateOffice365Domain.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}
