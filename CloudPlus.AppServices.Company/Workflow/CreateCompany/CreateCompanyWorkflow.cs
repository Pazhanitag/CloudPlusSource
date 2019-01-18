using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using CloudPlus.Constants;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Common;
using CloudPlus.QueueModels.Companies.Commands;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Company.Activities.AddCallbackRedirectUri;
using CloudPlus.Workflows.Company.Activities.AssignCatalog;
using CloudPlus.Workflows.Company.Activities.CompanyCreated;
using CloudPlus.Workflows.Company.Activities.CreateActiveDirectoryCompany;
using CloudPlus.Workflows.Company.Activities.CreateDatabaseCompany;
using CloudPlus.Workflows.Company.Mappers;
using CloudPlus.Workflows.User.Activities.CreateActiveDirectoryUser;
using CloudPlus.Workflows.User.Activities.CreateIdentityServerUser;
using CloudPlus.Workflows.User.Mappers;

namespace CloudPlus.AppServices.Company.Workflow.CreateCompany
{
    public class CreateCompanyWorkflow : ICreateCompanyWorkflow
    {
        private readonly IActivityCompanyArgumentsMapper _activityCompanyArgumentsMapper;
        private readonly IActivityUserArgumentsMapper _activityUserArgumentsMapper;
        private readonly IActivityConfigurator _activityConfigurator;

        public CreateCompanyWorkflow(IActivityCompanyArgumentsMapper activityCompanyArgumentsMapper, IActivityUserArgumentsMapper activityUserArgumentsMapper, IActivityConfigurator activityConfigurator)
        {
            _activityCompanyArgumentsMapper = activityCompanyArgumentsMapper;
            _activityUserArgumentsMapper = activityUserArgumentsMapper;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<ICreateCompanyCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var createCompanyCommand = context.Message;

            builder.AddActivity(CompanyServiceConstants.ActivityCreateAdCompany, _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateActiveDirectoryComapnyActivity)), _activityCompanyArgumentsMapper.MapActiveDirectoryCompanyArguments(createCompanyCommand));

            builder.AddActivity(CompanyServiceConstants.ActivityCreateDatabaseCompany, _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDatabaseCompanyActivity)), _activityCompanyArgumentsMapper.MapDatabaseCompanyArguments(createCompanyCommand));

            builder.AddActivity(CompanyServiceConstants.ActivityAssignCatalog, _activityConfigurator.GetActivityExecuteUri(context, typeof(IAssignCatalogActivity)), _activityCompanyArgumentsMapper.MapAssignCatalogArguments(createCompanyCommand));

            if (createCompanyCommand.Company.Type == CompanyType.Reseller)
            {
                builder.AddActivity(CompanyServiceConstants.ActivityAddCallbackRedirectUri,
                    _activityConfigurator.GetActivityExecuteUri(context, typeof(IAddCallbackRedirectUriActivity)),
                    new
                    {
                        Uri = createCompanyCommand.Company.ControlPanelSiteUrl,
                        createCompanyCommand.ClientDbId
                    });
            }

            builder.AddActivity(UserServiceConstants.ActivityCreateAdUser, _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateActiveDirectoryUserActivity)), _activityUserArgumentsMapper.MapActiveDirectoryUserArguments(createCompanyCommand));

            builder.AddActivity(UserServiceConstants.ActivityCreateIsUser, _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateIdentityServerUserActivity)), _activityUserArgumentsMapper.MapIdentityServerUserArguments(createCompanyCommand));

            builder.AddActivity(CompanyServiceConstants.ActivityCompanyCreated, _activityConfigurator.GetActivityExecuteUri(context, typeof(ICompanyCreatedActivity)), _activityCompanyArgumentsMapper.MapCreatedComapnySendEmailArguments(createCompanyCommand));

            builder.AddSubscription(CompanyServiceConstants.RoutingSlipEventObserverUri,
                RoutingSlipEvents.Completed |
                RoutingSlipEvents.Faulted |
                RoutingSlipEvents.ActivityCompleted |
                RoutingSlipEvents.ActivityFaulted |
                RoutingSlipEvents.ActivityCompensated |
                RoutingSlipEvents.ActivityCompensationFailed);

            var routingSlip = builder.Build();

            await context.Send<IRoutingSlipStarted>(CompanyServiceConstants.RoutingSlipCompanyStartedEventUri, new
            {
                builder.TrackingNumber,
                CreateTimestamp = DateTime.UtcNow,
                Arguments = context.Message,
                WorkflowActivityType = WorkflowActivityType.CreateCompany.ToString()
            });

            await context.Execute(routingSlip);
        }
    }
}
