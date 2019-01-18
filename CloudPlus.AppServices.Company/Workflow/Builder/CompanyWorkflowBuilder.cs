using Autofac.Core;
using Autofac.Core.Lifetime;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Common.Workflow;
using CloudPlus.Workflows.Company.Activities.AddCallbackRedirectUri;
using CloudPlus.Workflows.Company.Activities.AssignCatalog;
using CloudPlus.Workflows.Company.Activities.CompanyCreated;
using CloudPlus.Workflows.Company.Activities.CreateActiveDirectoryCompany;
using CloudPlus.Workflows.Company.Activities.CreateDatabaseCompany;
using CloudPlus.Workflows.Company.Activities.RemoveCallbackRedirectUri;
using CloudPlus.Workflows.Office365.Activities.Identity.RemoveCallbackRedirectUri;
using MassTransit.RabbitMqTransport;

namespace CloudPlus.AppServices.Company.Workflow.Builder
{
    public class CompanyWorkflowBuilder : IWorkflowBuilder
    {
        private readonly IActivityConfigurator _activityConfigurator;

        public CompanyWorkflowBuilder(IActivityConfigurator activityConfigurator)
        {
            _activityConfigurator = activityConfigurator;
        }
        public void BuildWorkflow(IRabbitMqBusFactoryConfigurator busFactoryConfigurator, IRabbitMqHost host,
            IComponentRegistry componentRegistry)
        {
            _activityConfigurator.ConfigureActivity<ICreateDatabaseCompanyActivity, ICreateDatabaseCompanyArguments, ICreateDatabaseCompanyLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));

            _activityConfigurator.ConfigureActivity<ICreateActiveDirectoryComapnyActivity, ICreateActiveDirectoryCompanyArguments, ICreateActiveDirectoryCompanyLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));

            _activityConfigurator.ConfigureExecuteActivity<ICompanyCreatedActivity, ICompanyCreatedArguments>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));

            _activityConfigurator.ConfigureActivity<IAssignCatalogActivity, IAssignCatalogArguments, IAssignCatalogLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));

            _activityConfigurator.ConfigureActivity<IAddCallbackRedirectUriActivity, IAddCallbackRedirectUriArguments, IAddCallbackRedirectUriLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));

            _activityConfigurator.ConfigureActivity<IRemoveCallbackRedirectUriActivity, IRemoveCallbackRedirectUriArguments, IRemoveCallbackRedirectUriLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
        }
    }
}
