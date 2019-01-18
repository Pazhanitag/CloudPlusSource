using System;
using Autofac;
using MassTransit;
using MassTransit.AutofacIntegration;
using MassTransit.Courier;
using MassTransit.RabbitMqTransport;

namespace CloudPlus.Workflows.Common.ActivityConfigurator
{
    public class ActivityConfigurator : IActivityConfigurator
    {
        public void ConfigureActivity<TActivity, TArguments, TLog>(IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host,
            ILifetimeScope lifetimeScope)
            where TActivity : class, Activity<TArguments, TLog>
            where TArguments : class
            where TLog : class
        {
            Uri compensateAddress = null;
            cfg.ReceiveEndpoint(host, GetCompensateActivityQueueName(typeof(TActivity)), e =>
            {
                e.PrefetchCount = 20;
                e.CompensateActivityHost(new AutofacCompensateActivityFactory<TActivity, TLog>(lifetimeScope));
                compensateAddress = e.InputAddress;
            });

            cfg.ReceiveEndpoint(host, GetExecuteActivityQueueName(typeof(TActivity)), e =>
            {
                e.PrefetchCount = 20;
                e.ExecuteActivityHost(compensateAddress, new AutofacExecuteActivityFactory<TActivity, TArguments>(lifetimeScope));
            });
        }

        public void ConfigureExecuteActivity<TActivity, TArguments>(IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host,
            ILifetimeScope lifetimeScope) 
            where TActivity : class, ExecuteActivity<TArguments> where TArguments : class
        {
            cfg.ReceiveEndpoint(host, GetExecuteActivityQueueName(typeof(TActivity)), e =>
            {
                e.PrefetchCount = 20;
                e.ExecuteActivityHost(new AutofacExecuteActivityFactory<TActivity, TArguments>(lifetimeScope));
            });
        }

        public void ConfigureExecuteActivity<TActivity, TArguments>(IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host,
            ILifetimeScope lifetimeScope, Action<IExecuteActivityConfigurator<TActivity, TArguments>> executeActivityConfigurator) 
            where TActivity : class, ExecuteActivity<TArguments> where TArguments : class
        {
            cfg.ReceiveEndpoint(host, GetExecuteActivityQueueName(typeof(TActivity)), e =>
            {
                e.PrefetchCount = 20;
                e.ExecuteActivityHost(new AutofacExecuteActivityFactory<TActivity, TArguments>(lifetimeScope), executeActivityConfigurator);
            });
        }
        public void ConfigureActivity<TActivity, TArguments, TLog>(IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host,
            ILifetimeScope lifetimeScope, Action<IExecuteActivityConfigurator<TActivity, TArguments>> executeActivityConfigurator, Action<ICompensateActivityConfigurator<TActivity, TLog>> compensateActivityConfigurator = null)
            where TActivity : class, Activity<TArguments, TLog>
            where TArguments : class
            where TLog : class
        {
            Uri compensateAddress = null;
            cfg.ReceiveEndpoint(host, GetCompensateActivityQueueName(typeof(TActivity)), e =>
            {
                e.PrefetchCount = 20;
                e.CompensateActivityHost(new AutofacCompensateActivityFactory<TActivity, TLog>(lifetimeScope), compensateActivityConfigurator);
                compensateAddress = e.InputAddress;
            });

            cfg.ReceiveEndpoint(host, GetExecuteActivityQueueName(typeof(TActivity)), e =>
            {
                e.PrefetchCount = 20;
                e.ExecuteActivityHost(compensateAddress, new AutofacExecuteActivityFactory<TActivity, TArguments>(lifetimeScope), executeActivityConfigurator);
            });
        }
        public Uri GetActivityExecuteUri(ConsumeContext context, Type activityType)
        {
            var builder = new UriBuilder
            {
                Scheme = context.SourceAddress.Scheme,
                Host = context.SourceAddress.Host,
                Path = GetExecuteActivityQueueName(activityType)
            };
            return builder.Uri;
        }
        private string GetExecuteActivityQueueName(Type activityType)
        {
            return $"execute-{activityType.Name.Replace("Activity", "").ToLowerInvariant()}";
        }

        private string GetCompensateActivityQueueName(Type activityType)
        {
            return $"compensate-{activityType.Name.Replace("Activity", "").ToLowerInvariant()}";
        }

        
    }
}