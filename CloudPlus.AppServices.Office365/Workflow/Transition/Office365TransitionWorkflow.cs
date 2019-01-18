using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using CloudPlus.Constants;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Common;
using CloudPlus.QueueModels.Office365.Transition.Commands;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseCustomer;
using CloudPlus.Workflows.Office365.Activities.Customer.MultiDatabaseCustomerSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.MultiPartnerPlatformCustomerSubscription;
using CloudPlus.Workflows.Office365.Activities.Domain.AddMultiDomainToDatabase;
using CloudPlus.Workflows.Office365.Activities.Transition.DatabaseProvisionedStatusProvisioned;
using CloudPlus.Workflows.Office365.Activities.Transition.TransitionDispatchCreatingUser;
using CloudPlus.Workflows.Office365.Mappers;

namespace CloudPlus.AppServices.Office365.Workflow.Transition
{
    public class Office365TransitionWorkflow : IOffice365TransitionWorkflow
    {
        private readonly IActivityOffice365TransitionArgumentsMapper _mapper;
        private readonly IActivityConfigurator _activityConfigurator;

        public Office365TransitionWorkflow(IActivityOffice365TransitionArgumentsMapper mapper, IActivityConfigurator activityConfigurator)
        {
            _mapper = mapper;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<IOffice365TransitionCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var command = context.Message;

            builder.AddActivity(Office365ServiceConstants.ActivityCreateOffice365DatabaseCustomer,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDatabaseCustomerActivity)), _mapper.MapCreateDatabaseCustomerArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityAddMultiDomainToDatabase,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IAddMultiDomainToDatabaseActivity)), _mapper.MapAddMultiDomainToDatabaseArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityMultiPartnerPlatformCustomerSubscription,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IMultiPartnerPlatformCustomerSubscriptionActivity)), _mapper.MapMultiPartnerPlatformCustomerSubscriptionArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityMultiDatabaseCustomerSubscription,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IMultiDatabaseCustomerSubscriptionActivity)), _mapper.MapMultiDatabaseCustomerSubscriptionArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityDatabaseProvisionedStatusProvisioned,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IDatabaseProvisionedStatusProvisionedActivity)), _mapper.MapDatabaseProvisionedStatusProvisionedArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityTransitionDispatchCreatingUsers,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ITransitionDispatchCreatingUsersActivity)), _mapper.MapTransitionDispatchCreatingUserArguments(command));

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
                WorkflowActivityType = WorkflowActivityType.Office365Transition.ToString()
            });

            await context.Send<IOffice365TransitionReportCommand>(Office365ServiceConstants.QueueOffice365TransitioReportUri, new
            {
                command.CompanyId,
                command.ProductItems
            });

            await context.Execute(routingSlip);
        }
    }
}
