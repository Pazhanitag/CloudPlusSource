using System.Threading.Tasks;
using MassTransit;
using CloudPlus.Models.Enums;
using CloudPlus.Models.WorkflowActivity;
using CloudPlus.QueueModels.Common;
using CloudPlus.Services.Database.WorkflowActivity;

namespace CloudPlus.Workflows.Common.Consumers
{
    public class RoutingSlipStartedConsumer : IRoutingSlipStartedConsumer
    {
        private readonly IWorkflowActivityService _workflowActivityService;

        public RoutingSlipStartedConsumer(IWorkflowActivityService workflowActivityService)
        {
            _workflowActivityService = workflowActivityService;
        }

        public Task Consume(ConsumeContext<IRoutingSlipStarted> context)
        {
            var data = context.Message.Arguments;
            data.Add("workflowActivityType", context.Message.WorkflowActivityType.ToString());

            _workflowActivityService.Insert(new WorkflowActivityDto
            {
                UniqueId = context.ConversationId.ToString(),
                Context = new WorkflowActivityContentDto
                {
                    TrackingNumber = context.Message.TrackingNumber,
                    Timestamp = context.Message.CreateTimestamp,
                    Data = data,
                    WorkflowActivityStatus = WorkflowActivityStatus.RoutingSlipStart
                }
            });

            return Task.CompletedTask;
        }
    }
}
