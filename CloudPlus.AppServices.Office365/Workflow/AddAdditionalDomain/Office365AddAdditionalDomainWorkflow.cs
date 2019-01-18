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
using CloudPlus.Workflows.Office365.Activities.Domain.AddCustomerDomainPartnerPortalActivity;
using CloudPlus.Workflows.Office365.Activities.Domain.AddCustomerDomainToDatabaseActivity;
using CloudPlus.Workflows.Office365.Activities.Domain.GetCustomerTxtRecords;
using CloudPlus.Workflows.Office365.Activities.Domain.SendCustomerTxtRecords;
using CloudPlus.Workflows.Office365.Mappers;

namespace CloudPlus.AppServices.Office365.Workflow.AddAdditionalDomain
{
    public class Office365AddAdditionalDomainWorkflow : IOffice365AddAdditionalDomainWorkflow
    {
        private readonly IActivityOffice365AddAdditionalDomainArgumentsMapper _mapper;
        private readonly IActivityConfigurator _activityConfigurator;

        public Office365AddAdditionalDomainWorkflow(IActivityOffice365AddAdditionalDomainArgumentsMapper mapper, IActivityConfigurator activityConfigurator)
        {
            _mapper = mapper;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<IOffice365AddAdditionalDomainCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var command = context.Message;

            builder.AddActivity(Office365ServiceConstants.ActivityAddOffice365CustomerDomainToPartnerPortal, 
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IAddCustomerDomainPartnerPortalActivity)), 
                _mapper.MapAddCustomerDomainPartnerPortalArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityAddOffice365CustomerDomainToDatabase,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IAddCustomerDomainToDatabaseActivity)), 
                _mapper.MapAddCustomerDomainToDatabaseArguments(command));

            builder.AddActivity(Office365ServiceConstants.ActivityGetOffice365CustomerTxtRecords, 
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IGetCustomerDomainTxtRecordsActivity)), 
                _mapper.MapGetCustomerTxtRecords(command));

            builder.AddActivity(Office365ServiceConstants.ActivitySendOffice365CustomerTxtRecords, 
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ISendCustomerDomainTxtRecordsActivity)), 
                _mapper.MapSendCustomerTxtRecords(command));

            builder.AddVariable("domain", context.Message.Domain);
            builder.AddVariable("workflowActivityType", WorkflowActivityType.Office365AddAdditionalDomain.ToString());

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
                WorkflowActivityType = WorkflowActivityType.Office365AddAdditionalDomain.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}
