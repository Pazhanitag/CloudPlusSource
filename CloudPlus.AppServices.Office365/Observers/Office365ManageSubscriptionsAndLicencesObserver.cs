using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using CloudPlus.Models.Enums;
using CloudPlus.Models.WorkflowActivity;
using CloudPlus.QueueModels.Office365.Subscriptions.Commands;
using CloudPlus.Services.Database.WorkflowActivity;

namespace CloudPlus.AppServices.Office365.Observers
{
    public class Office365ManageSubscriptionsAndLicencesObserver : IOffice365ManageSubscriptionsAndLicencesObserver
    {
        private readonly IWorkflowActivityService _workflowActivityService;

        public Office365ManageSubscriptionsAndLicencesObserver(IWorkflowActivityService workflowActivityService)
        {
            _workflowActivityService = workflowActivityService;
        }

        public Task PreConsume(ConsumeContext<IManageSubscriptionsAndLicencesCommand> context)
        {
            foreach (var user in context.Message.Users)
            {
                var usersDictionary = new Dictionary<string, object>
                {
                    {"userPrincipalName", user.UserPrincipalName},
                    {"workflowActivityType", WorkflowActivityType.Office365ManageSubscription.ToString()},
                    {"messageType", context.Message.MessageType}
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
            }

            return Task.FromResult(context);
        }

        public Task PostConsume(ConsumeContext<IManageSubscriptionsAndLicencesCommand> context)
        {
            return Task.FromResult(context);
        }

        public Task ConsumeFault(ConsumeContext<IManageSubscriptionsAndLicencesCommand> context, Exception exception)
        {
            return Task.FromResult(context);
        }
    }
}
