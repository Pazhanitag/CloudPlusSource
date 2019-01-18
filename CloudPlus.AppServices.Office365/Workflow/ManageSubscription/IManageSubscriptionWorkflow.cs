using CloudPlus.QueueModels.Office365.Subscriptions.Commands;
using CloudPlus.Workflows.Common.Workflow;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Workflow.ManageSubscription
{
    public interface IManageSubscriptionWorkflow : IWorkflow<ConsumeContext<IManageSubscriptionsAndLicencesCommand>>
    {

    }
}