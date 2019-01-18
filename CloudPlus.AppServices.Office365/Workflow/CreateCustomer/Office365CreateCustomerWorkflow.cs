using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using CloudPlus.Constants;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Common;
using CloudPlus.QueueModels.Office365.Customer.Commands;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseCustomer;
using CloudPlus.Workflows.Office365.Activities.Customer.CreatePartnerPlatformCustomer;
using CloudPlus.Workflows.Office365.Activities.Domain.AddCustomerDomainPartnerPortalActivity;
using CloudPlus.Workflows.Office365.Activities.Domain.AddCustomerDomainToDatabaseActivity;
using CloudPlus.Workflows.Office365.Activities.Domain.GetCustomerTxtRecords;
using CloudPlus.Workflows.Office365.Activities.Domain.SendCustomerTxtRecords;
using CloudPlus.Workflows.Office365.Mappers;

namespace CloudPlus.AppServices.Office365.Workflow.CreateCustomer
{
    public class Office365CreateCustomerWorkflow : IOffice365CreateCustomerWorkflow
    {
        private readonly IActivityOffice365CustomerArgumentsMapper _activityOffice365CustomerArgumentsMapper;
        private readonly IActivityConfigurator _activityConfigurator;

        public Office365CreateCustomerWorkflow(IActivityOffice365CustomerArgumentsMapper activityOffice365CustomerArgumentsMapper, IActivityConfigurator activityConfigurator)
        {
            _activityOffice365CustomerArgumentsMapper = activityOffice365CustomerArgumentsMapper;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<IOffice365CreateCustommerCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var createOffice365CustommerCommand = context.Message;

            builder.AddActivity(Office365ServiceConstants.ActivityCreateOffice365PartnerPlatformCustomer,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreatePartnerPlatformCustomerActivity)),
                _activityOffice365CustomerArgumentsMapper.MapCreatePartnerPlatformCustomerArguments(createOffice365CustommerCommand));

            builder.AddActivity(Office365ServiceConstants.ActivityCreateOffice365DatabaseCustomer,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDatabaseCustomerActivity)),
                _activityOffice365CustomerArgumentsMapper.MapCreateDatabaseCustomerArguments(createOffice365CustommerCommand));

            builder.AddActivity(Office365ServiceConstants.ActivityAddOffice365CustomerDomainToPartnerPortal,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IAddCustomerDomainPartnerPortalActivity)),
                _activityOffice365CustomerArgumentsMapper.MapAddCustomerDomainPartnerPortalArguments(createOffice365CustommerCommand));

            builder.AddActivity(Office365ServiceConstants.ActivityAddOffice365CustomerDomainToDatabase,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IAddCustomerDomainToDatabaseActivity)),
                _activityOffice365CustomerArgumentsMapper.MapAddCustomerDomainToDatabaseArguments(createOffice365CustommerCommand));

            builder.AddActivity(Office365ServiceConstants.ActivityGetOffice365CustomerTxtRecords,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(IGetCustomerDomainTxtRecordsActivity)),
                _activityOffice365CustomerArgumentsMapper.MapGetCustomerTxtRecords(createOffice365CustommerCommand));

            builder.AddActivity(Office365ServiceConstants.ActivitySendOffice365CustomerTxtRecords,
                _activityConfigurator.GetActivityExecuteUri(context, typeof(ISendCustomerDomainTxtRecordsActivity)),
                _activityOffice365CustomerArgumentsMapper.MapSendCustomerTxtRecords(createOffice365CustommerCommand));

            builder.AddVariable("companyId", context.Message.CompanyId);
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
                WorkflowActivityType = WorkflowActivityType.CreateOffice365Customer.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}
