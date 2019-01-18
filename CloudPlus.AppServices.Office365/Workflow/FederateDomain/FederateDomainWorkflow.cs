using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using CloudPlus.Constants;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Common;
using CloudPlus.QueueModels.Office365.Domain.Federate;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Office365.Activities.Domain.FederateCustomerDomain;
using CloudPlus.Workflows.Office365.Activities.Domain.FederateCustomerDomainDatabaseStatus;
using CloudPlus.Workflows.Office365.Activities.User.AssignUserRoles;
using CloudPlus.Workflows.Office365.Activities.User.CreateTempPartnerPlatformAdminUser;
using CloudPlus.Workflows.Office365.Activities.User.HardDeletePartnerPlatformUser;

namespace CloudPlus.AppServices.Office365.Workflow.FederateDomain
{
    public class FederateDomainWorkflow : IFederateDomainWorkflow
    {
        private readonly IActivityConfigurator _activityConfigurator;
        private readonly IOffice365DbCustomerService _office365DbCustomerService;

        public FederateDomainWorkflow(IActivityConfigurator activityConfigurator,
            IOffice365DbCustomerService office365DbCustomerService)
        {
            _activityConfigurator = activityConfigurator;
            _office365DbCustomerService = office365DbCustomerService;
        }

        public async Task Execute(ConsumeContext<IOffice365FederateDomainRequest> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var command = context.Message;
            var o365Customer = await _office365DbCustomerService.GetOffice365CustomerAsync(command.CompanyId);

            builder.AddActivity(Office365ServiceConstants.ActivityCreateTempAdminUser,
                _activityConfigurator.GetActivityExecuteUri(context,
                    typeof(ICreateTempPartnerPlatformAdminUserActivity)),
                new
                {
                    o365Customer.Office365CustomerId,
                    WorkflowActivityType = WorkflowActivityType.FederateOffice365Domain,
                    WorkflowStep = WorkflowActivityStep.Office365CreateTempPartnerPlatformAdminUser
                });

            builder.AddActivity(Office365ServiceConstants.ActivityAssignUserRoles,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignUserRolesActivity)),
                new
                {
                    o365Customer.Office365CustomerId,
                    WorkflowActivityType = WorkflowActivityType.FederateOffice365Domain,
                    WorkflowStep = WorkflowActivityStep.Office365AssignUserRoles
                });

            builder.AddActivity(Office365ServiceConstants.ActivityFederateCustomerDomain,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IFederateCustomerDomainActivity)),
                new
                {
                    o365Customer.Office365CustomerId,
                    command.DomainName,
                    WorkflowActivityType = WorkflowActivityType.FederateOffice365Domain,
                    WorkflowStep = WorkflowActivityStep.Office365FederateCustomerDomain
                });

            builder.AddActivity(Office365ServiceConstants.ActivityFederateCustomerDomainDatabaseStatus,
                _activityConfigurator.GetActivityExecuteUri(context,
                    typeof(IFederateCustomerDomainDatabaseStatusActivity)),
                new
                {
                    o365Customer.Office365CustomerId,
                    command.DomainName,
                    WorkflowActivityType = WorkflowActivityType.FederateOffice365Domain,
                    WorkflowStep = WorkflowActivityStep.Office365FederateCustomerDomainDatabaseStatus
                });

            builder.AddActivity(Office365ServiceConstants.ActivityHardDeletePartnerPlatformUser,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IHardDeletePartnerPlatformUserActivity)),
                new
                {
                    o365Customer.Office365CustomerId,
                    SwallowException = true,
                    WorkflowActivityType = WorkflowActivityType.FederateOffice365Domain,
                    WorkflowStep = WorkflowActivityStep.Office365HardDeletePartnerPortalUser
                });

            builder.AddSubscription(Office365ServiceConstants.QueueOffice365RoutingSlipEventUri,
                RoutingSlipEvents.Completed |
                RoutingSlipEvents.Faulted |
                RoutingSlipEvents.ActivityCompleted |
                RoutingSlipEvents.ActivityFaulted |
                RoutingSlipEvents.ActivityCompensated |
                RoutingSlipEvents.ActivityCompensationFailed);

            await builder.AddSubscription(context.ResponseAddress, RoutingSlipEvents.Completed, endpoint =>
                endpoint.Send(new Office365FederateDomainResponse {IsDomainFederated = true},
                    sendContext => sendContext.RequestId = context.RequestId));

            await builder.AddSubscription(context.ResponseAddress, RoutingSlipEvents.Faulted, endpoint =>
                endpoint.Send(new Office365FederateDomainResponse {IsDomainFederated = false},
                    sendContext => sendContext.RequestId = context.RequestId));

            var routingSlip = builder.Build();

            await context.Send<IRoutingSlipStarted>(Office365ServiceConstants.QueueOffice365RoutingSlipStartedUri, new
            {
                builder.TrackingNumber,
                CreateTimestamp = DateTime.UtcNow,
                Arguments = context.Message,
                WorkflowActivityType = WorkflowActivityType.FederateOffice365Domain.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}