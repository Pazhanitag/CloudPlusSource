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
using CloudPlus.Workflows.Office365.Activities.Domain.GetCustomerTxtRecords;
using CloudPlus.Workflows.Office365.Activities.Domain.SendCustomerTxtRecords;
using CloudPlus.Workflows.Office365.Mappers;

namespace CloudPlus.AppServices.Office365.Workflow.ResendTxtRecords
{
    public class Office365ResendTxtRecordsWorkflow : IOffice365ResendTxtRecordsWorkflow
    {
        private readonly IActivityOffice365CustomerArgumentsMapper _activityOffice365CustomerArgumentsMapper;
        private readonly IActivityConfigurator _activityConfigurator;

        public Office365ResendTxtRecordsWorkflow(IActivityOffice365CustomerArgumentsMapper activityOffice365CustomerArgumentsMapper, IActivityConfigurator activityConfigurator)
        {
            _activityOffice365CustomerArgumentsMapper = activityOffice365CustomerArgumentsMapper;
            _activityConfigurator = activityConfigurator;
        }
        public async Task Execute(ConsumeContext<IOffice365ResendTxtRecordsCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var resendTxtRecordsCommand = context.Message;

            builder.AddActivity(Office365ServiceConstants.ActivityGetOffice365CustomerTxtRecords, _activityConfigurator.GetActivityExecuteUri(context, typeof(IGetCustomerDomainTxtRecordsActivity)), _activityOffice365CustomerArgumentsMapper.MapGetCustomerTxtRecords(resendTxtRecordsCommand));
            builder.AddActivity(Office365ServiceConstants.ActivitySendOffice365CustomerTxtRecords, _activityConfigurator.GetActivityExecuteUri(context, typeof(ISendCustomerDomainTxtRecordsActivity)), _activityOffice365CustomerArgumentsMapper.MapSendCustomerTxtRecords(resendTxtRecordsCommand));

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
                WorkflowActivityType = WorkflowActivityType.ResendOffice365DomainTxtRecords.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}
