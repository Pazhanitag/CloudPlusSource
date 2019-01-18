using System;
using System.Threading.Tasks;
using MassTransit;
using CloudPlus.Api.NotificationService.Services;
using CloudPlus.Enums.Notification;
using CloudPlus.Models.Notification;
using CloudPlus.QueueModels.EmailNotification.Commands;

namespace CloudPlus.Api.NotificationService.Consumers
{
    public class SendEmailUserConsumer : IConsumer<ISendEmailUserCommand>
    {
        private readonly IEmailMessageService _emailMessageService;
        private readonly ISendEmailService _sendEmailService;

        public SendEmailUserConsumer(IEmailMessageService emailMessageService, ISendEmailService sendEmailService)
        {
            _emailMessageService = emailMessageService;
            _sendEmailService = sendEmailService;
        }

        public Task Consume(ConsumeContext<ISendEmailUserCommand> context)
        {
            var emailData = context.Message;
            var template = EmailTemplateFactory.GetEmailTemplate(_emailMessageService, emailData);

            var message = new EmailMessageModel
            {
                To = emailData.To,
                Subject = template.Subject,
                Body = template.Body,
                EmailMediaType = EmailMediaType.Html
            };

            _sendEmailService.SendEmail(message);

            return Task.FromResult(context.Message);
        }
    }
}
