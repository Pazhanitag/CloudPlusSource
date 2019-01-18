using System;
using Autofac;
using MassTransit;
using MassTransit.Courier;
using MassTransit.RabbitMqTransport;

namespace CloudPlus.Workflows.Common.ActivityConfigurator
{
    public interface IActivityConfigurator
    {
        void ConfigureActivity<TActivity, TArguments, TLog>(IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host, ILifetimeScope lifetimeScope)
            where TActivity : class, Activity<TArguments, TLog>
            where TArguments : class
            where TLog : class;

        void ConfigureExecuteActivity<TActivity, TArguments>(IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host, ILifetimeScope lifetimeScope)
            where TActivity : class, ExecuteActivity<TArguments>
            where TArguments : class;

        void ConfigureActivity<TActivity, TArguments, TLog>(IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host,
            ILifetimeScope lifetimeScope, Action<IExecuteActivityConfigurator<TActivity, TArguments>> executeActivityConfigurator, Action<ICompensateActivityConfigurator<TActivity, TLog>> compensateActivityConfigurator = null)
            where TActivity : class, Activity<TArguments, TLog>
            where TArguments : class
            where TLog : class;

        void ConfigureExecuteActivity<TActivity, TArguments>(IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host,
            ILifetimeScope lifetimeScope,
            Action<IExecuteActivityConfigurator<TActivity, TArguments>> executeActivityConfigurator)
            where TActivity : class, ExecuteActivity<TArguments> where TArguments : class;

        Uri GetActivityExecuteUri(ConsumeContext context, Type activityType);
    }
}