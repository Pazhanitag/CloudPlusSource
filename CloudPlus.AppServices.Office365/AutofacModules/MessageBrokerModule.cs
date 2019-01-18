using Autofac;
using MassTransit;
using CloudPlus.AppServices.Office365.Observers;
using CloudPlus.Constants;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Infrastructure.ServiceBus.RabbitMq;
using CloudPlus.Workflows.Common.Workflow;

namespace CloudPlus.AppServices.Office365.AutofacModules
{
    public class MessageBrokerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                {
                    var messageBrokerConfigurator = context.Resolve<IRabbitMqMessageBroker>();
                    var workflowBuilder = context.Resolve<IWorkflowBuilder>();
                    var office365Observer = context.Resolve<IOffice365ManageSubscriptionsAndLicencesObserver>();

                    var messageBroker = messageBrokerConfigurator.ConfigureBus((busFactoryConfigurator, host) =>
                    {
                        workflowBuilder.BuildWorkflow(busFactoryConfigurator, host, context.ComponentRegistry);

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueCreateOffice365Customer,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365ResendTxtRecords,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365AddressValidation,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365DomainVerification,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365UserAssignLicense,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365UserChangeLicense,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365UserRemoveLicense,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365UserRestore,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice3655UserMultiEdit,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueAddAdditionalOffice365Domain,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365ChangeUserRoles,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365GetUserRoles,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365Transition,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365TransitionUserAndLicense,
                            endpointConfigurator =>
                            {
                                endpointConfigurator.PrefetchCount = 1;
                                endpointConfigurator.LoadFrom(context);
                            });

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365TransitionDeletePartnerPlatformUser,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365TransitioReport,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365HardDeleteUser,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365CreateUser,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));
                        
                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueManageSubscriptionsAndLicences,
                            endpointConfigurator =>
                            {
                                endpointConfigurator.PrefetchCount = 1;
                                endpointConfigurator.LoadFrom(context);
                            });

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365RoutingSlipEventRoute,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365RoutingSlipStartedRoute,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.Office365ManageSubscriptionsAndLicencesObserverRoute,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, Office365ServiceConstants.QueueOffice365TransitionDeletePartnerPlatformUserObserverRoute,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));
                    });

                    messageBroker.Bus().ConnectConsumeMessageObserver(office365Observer);

                    return messageBroker;
                })
                .SingleInstance()
                .As<IMessageBroker>();
        }
    }
}
