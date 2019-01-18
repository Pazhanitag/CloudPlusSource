using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using CloudPlus.Models.Enums;
using CloudPlus.Models.WorkflowActivity;
using CloudPlus.QueueModels.Office365.Transition.Commands;
using CloudPlus.Services.Database.WorkflowActivity;

namespace CloudPlus.AppServices.Office365.Observers
{
    public class Office365TransitionDeletePartnerPlatformUserObserver : IOffice365TransitionDeletePartnerPlatformUserObserver
    {
        private readonly IWorkflowActivityService _workflowActivityService;

        public Office365TransitionDeletePartnerPlatformUserObserver(IWorkflowActivityService workflowActivityService)
        {
            _workflowActivityService = workflowActivityService;
        }

        public Task PreConsume(ConsumeContext<IOffice365TransitionDeletePartnerPlatformUserCommand> context)
        {
            var usersDictionary = new Dictionary<string, object>
            {
                {"office365UserId", context.Message.Office365UserId},
                {"workflowActivityType", WorkflowActivityType.Office365TransitionDeletePartnerPlatformUser.ToString()}
            };

            _workflowActivityService.Insert(new WorkflowActivityDto
            {
                UniqueId = context.ConversationId.ToString(),
                Context = new WorkflowActivityContentDto
                {
                    Timestamp = DateTime.UtcNow,
                    Data = usersDictionary,
                    WorkflowActivityStatus = WorkflowActivityStatus.RoutingSlipStart
                }
            });

            return Task.FromResult(context);
        }

        public Task PostConsume(ConsumeContext<IOffice365TransitionDeletePartnerPlatformUserCommand> context)
        {
            var usersDictionary = new Dictionary<string, object>
            {
                {"office365UserId", context.Message.Office365UserId},
                {"workflowActivityType", WorkflowActivityType.Office365TransitionDeletePartnerPlatformUser.ToString()}
            };

            _workflowActivityService.Insert(new WorkflowActivityDto
            {
                UniqueId = context.ConversationId.ToString(),
                Context = new WorkflowActivityContentDto
                {
                    Timestamp = DateTime.UtcNow,
                    Data = usersDictionary,
                    WorkflowActivityStatus = WorkflowActivityStatus.RoutingSlipCompleted
                }
            });

            return Task.FromResult(context);
        }

        public Task ConsumeFault(ConsumeContext<IOffice365TransitionDeletePartnerPlatformUserCommand> context, Exception exception)
        {
            var usersDictionary = new Dictionary<string, object>
            {
                {"office365UserId", context.Message.Office365UserId},
                {"workflowActivityType", WorkflowActivityType.Office365TransitionDeletePartnerPlatformUser.ToString()}
            };

            _workflowActivityService.Insert(new WorkflowActivityDto
            {
                UniqueId = context.ConversationId.ToString(),
                Context = new WorkflowActivityContentDto
                {
                    Timestamp = DateTime.UtcNow,
                    Data = usersDictionary,
                    WorkflowActivityStatus = WorkflowActivityStatus.RoutingSlipFaulted
                }
            });

            return Task.FromResult(context);
        }
    }
}
