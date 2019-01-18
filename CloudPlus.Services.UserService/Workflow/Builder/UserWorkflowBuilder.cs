using Autofac.Core;
using Autofac.Core.Lifetime;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Common.Workflow;
using CloudPlus.Workflows.User.Activities.CreateActiveDirectoryUser;
using CloudPlus.Workflows.User.Activities.CreateIdentityServerUser;
using CloudPlus.Workflows.User.Activities.DeleteActiveDirectoryUser;
using CloudPlus.Workflows.User.Activities.DeleteIdentityServerUser;
using CloudPlus.Workflows.User.Activities.UpdateActiveDirectoryUser;
using CloudPlus.Workflows.User.Activities.UpdateIdentityServerUser;
using CloudPlus.Workflows.User.Activities.UserCreated;
using MassTransit.RabbitMqTransport;

namespace CloudPlus.AppServices.User.Workflow.Builder
{
    public class UserWorkflowBuilder : IWorkflowBuilder
    {
        private readonly IActivityConfigurator _activityConfigurator;

        public UserWorkflowBuilder(IActivityConfigurator activityConfigurator)
        {
            _activityConfigurator = activityConfigurator;
        }
        
        public void BuildWorkflow(IRabbitMqBusFactoryConfigurator busFactoryConfigurator, IRabbitMqHost host,
            IComponentRegistry componentRegistry)
        {
            _activityConfigurator.ConfigureActivity<ICreateActiveDirectoryUserActivity, ICreateActiveDirectoryUserArguments, ICreateActiveDirectoryUserLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator.ConfigureActivity<ICreateIdentityServerUserActivity, ICreateIdentityServerUserArguments, ICreateIdentityServerUserLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator.ConfigureActivity<IUpdateActiveDirectoryUserActivity, IUpdateActiveDirectoryUserArguments, IUpdateActiveDirectoryUserLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator.ConfigureActivity<IDeleteActiveDirectoryUserActivity, IDeleteActiveDirectoryUserArguments, IDeleteActiveDirectoryUserLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator.ConfigureActivity<IDeleteIdentityServerUserActivity, IDeleteIdentityServerUserArguments, IDeleteIdentityServerUserLog>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));

            _activityConfigurator.ConfigureExecuteActivity<IUserCreatedActivity, IUserCreatedArguments>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
            _activityConfigurator.ConfigureExecuteActivity<IUpdateIdentityServerUserActivity, IUpdateIdentityServerUserArguments>(busFactoryConfigurator, host, new LifetimeScope(componentRegistry));
        }
    }
}
