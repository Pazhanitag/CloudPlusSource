using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier.Contracts;
using CloudPlus.Models.Enums;
using CloudPlus.Models.WorkflowActivity;
using CloudPlus.Services.Database.WorkflowActivity;

namespace CloudPlus.Workflows.Common.Consumers
{
    public class WorkflowActivityConsumer : IWorkflowActivityConsumer
    {
        private readonly IWorkflowActivityService _workflowActivityService;

        public WorkflowActivityConsumer(IWorkflowActivityService workflowActivityService)
        {
            _workflowActivityService = workflowActivityService;
        }

        public Task Consume(ConsumeContext<RoutingSlipActivityCompensated> context)
        {
            _workflowActivityService.Insert(new WorkflowActivityDto
            {
                UniqueId = context.ConversationId.ToString(),
                Context = new WorkflowActivityContentDto
                {
                    TrackingNumber = context.Message.TrackingNumber,
                    Timestamp = context.Message.Timestamp,
                    ExecutionId = context.Message.ExecutionId,
                    ActivityName = context.Message.ActivityName,
                    Duration = context.Message.Duration,
                    WorkflowActivityStatus = WorkflowActivityStatus.ActivityCompensated,
                    Data = context.Message.Variables
                }
            });

            return Task.FromResult(context);
        }

        public Task Consume(ConsumeContext<RoutingSlipActivityCompensationFailed> context)
        {
            _workflowActivityService.Insert(new WorkflowActivityDto
            {
                UniqueId = context.ConversationId.ToString(),
                Context = new WorkflowActivityContentDto
                {
                    TrackingNumber = context.Message.TrackingNumber,
                    Timestamp = context.Message.Timestamp,
                    ExecutionId = context.Message.ExecutionId,
                    ActivityName = context.Message.ActivityName,
                    Duration = context.Message.Duration,
                    WorkflowActivityStatus = WorkflowActivityStatus.ActivityCompensationFailed,
                    Data = context.Message.Variables,
                    Exception = context.Message.ExceptionInfo != null
                        ? new WorkflowActivityExceptionDto
                        {
                            ExceptionType = context.Message.ExceptionInfo.ExceptionType,
                            Message = context.Message.ExceptionInfo.Message,
                            InnerMessage = context.Message.ExceptionInfo.InnerException.Message,
                            Source = context.Message.ExceptionInfo.Source,
                            StackTrace = context.Message.ExceptionInfo.StackTrace
                        }
                        : null
                }
            });

            return Task.FromResult(context);
        }

        public Task Consume(ConsumeContext<RoutingSlipActivityCompleted> context)
        {
            _workflowActivityService.Insert(new WorkflowActivityDto
            {
                UniqueId = context.ConversationId.ToString(),
                Context = new WorkflowActivityContentDto
                {
                    TrackingNumber = context.Message.TrackingNumber,
                    Timestamp = context.Message.Timestamp,
                    ExecutionId = context.Message.ExecutionId,
                    ActivityName = context.Message.ActivityName,
                    Duration = context.Message.Duration,
                    WorkflowActivityStatus = WorkflowActivityStatus.ActivityCompleted,
                    Data = context.Message.Arguments
                }
            });

            return Task.FromResult(context);
        }

        public Task Consume(ConsumeContext<RoutingSlipActivityFaulted> context)
        {
            _workflowActivityService.Insert(new WorkflowActivityDto
            {
                UniqueId = context.ConversationId.ToString(),
                Context = new WorkflowActivityContentDto
                {
                    TrackingNumber = context.Message.TrackingNumber,
                    Timestamp = context.Message.Timestamp,
                    ExecutionId = context.Message.ExecutionId,
                    ActivityName = context.Message.ActivityName,
                    Duration = context.Message.Duration,
                    WorkflowActivityStatus = WorkflowActivityStatus.ActivityFaulted,
                    Data = context.Message.Arguments,
                    Exception = context.Message.ExceptionInfo != null
                        ? new WorkflowActivityExceptionDto
                        {
                            ExceptionType = context.Message.ExceptionInfo.ExceptionType,
                            Message = context.Message.ExceptionInfo.Message,
                            InnerMessage = context.Message.ExceptionInfo.InnerException?.Message,
                            Source = context.Message.ExceptionInfo.Source,
                            StackTrace = context.Message.ExceptionInfo.StackTrace
                        }
                        : null
                }
            });

            return Task.FromResult(context);
        }

        public Task Consume(ConsumeContext<RoutingSlipCompleted> context)
        {
            _workflowActivityService.Insert(new WorkflowActivityDto
            {
                UniqueId = context.ConversationId.ToString(),
                Context = new WorkflowActivityContentDto
                {
                    TrackingNumber = context.Message.TrackingNumber,
                    Timestamp = context.Message.Timestamp,
                    Duration = context.Message.Duration,
                    WorkflowActivityStatus = WorkflowActivityStatus.RoutingSlipCompleted,
                    Data = context.Message.Variables
                }
            });

            return Task.FromResult(context);
        }

        public Task Consume(ConsumeContext<RoutingSlipFaulted> context)
        {
            _workflowActivityService.Insert(new WorkflowActivityDto
            {
                UniqueId = context.ConversationId.ToString(),
                Context = new WorkflowActivityContentDto
                {
                    TrackingNumber = context.Message.TrackingNumber,
                    Timestamp = context.Message.Timestamp,
                    Duration = context.Message.Duration,
                    WorkflowActivityStatus = WorkflowActivityStatus.RoutingSlipFaulted,
                    Data = context.Message.Variables,
                    Exception = context.Message.ActivityExceptions != null
                        ? new WorkflowActivityExceptionDto
                        {
                            ExceptionType = context.Message.ActivityExceptions[0].ExceptionInfo.ExceptionType,//TODO Could here be more then one exception?
                            Message = context.Message.ActivityExceptions[0].ExceptionInfo.Message,
                            InnerMessage = context.Message.ActivityExceptions[0].ExceptionInfo.InnerException?.Message,
                            Source = context.Message.ActivityExceptions[0].ExceptionInfo.Source,
                            StackTrace = context.Message.ActivityExceptions[0].ExceptionInfo.StackTrace
                        }
                        : null
                }
            });

            return Task.FromResult(context);
        }
    }
}
