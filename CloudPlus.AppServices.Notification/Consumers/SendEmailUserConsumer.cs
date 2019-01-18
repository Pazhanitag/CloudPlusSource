using System.Threading.Tasks;
using Autofac;
using MassTransit;
using CloudPlus.AppServices.Notification.Services;
using CloudPlus.Enums.Notification;
using CloudPlus.Logging;
using CloudPlus.Models.Notification;
using CloudPlus.QueueModels.EmailNotification.Commands;

namespace CloudPlus.AppServices.Notification.Consumers
{
    public class SendEmailUserConsumer : ISendEmailUserConsumer
    {
        private readonly IEmailMessageService _emailMessageService;

        public SendEmailUserConsumer(IEmailMessageService emailMessageService)
        {
            _emailMessageService = emailMessageService;
        }

        public async Task Consume(ConsumeContext<ISendEmailCommand> context)
        {
            var sendEmailCommand = context.Message;
            var message = new EmailMessageModel();
            if (sendEmailCommand.Attachment)
            {
                var attach = await _emailMessageService.SendProductDetailsAsEmail(sendEmailCommand.CompanyId, sendEmailCommand.CatalogId);
                var messageModel = new EmailMessageModel
                {
                    To = sendEmailCommand.To,
                    Subject = sendEmailCommand.Subject,
                    Body = sendEmailCommand.Body,
                    attachment = attach,
                };
                message = messageModel;
            }
            else
            {
                var emailData = new EmailTemplatePlaceholdersGeneratorRequestModel
                {
                    EmailTemplateType = sendEmailCommand.EmailTemplateType,
                    UserName = sendEmailCommand.UserName,
                    CompanyId = sendEmailCommand.CompanyId,
                    Password = sendEmailCommand.Password,
                    TempResetLink = sendEmailCommand.TempResetLink,
                    Domain = sendEmailCommand.Domain,
                    TxtRecord = sendEmailCommand.TxtRecord,
                    Report = sendEmailCommand.Report,
                    CustomSecurePanelId = sendEmailCommand.CustomSecureCompanyId,
                };

                var template = _emailMessageService.GetEmailTemplate(emailData);

                var MessageModel = new EmailMessageModel
                {
                    To = sendEmailCommand.To,
                    Subject = template.Subject,
                    Body = template.Body,
                    EmailMediaType = EmailMediaType.Html,
                    BCC = sendEmailCommand.BCC
                };
                message = MessageModel;
            }
            // Required fields: EmailTemplateType and UserName or ComapnyId
            var sendEmailService = Settings.IoC.GetContainer().Resolve<ISendEmailService>();

            this.Log().Info($"Sending email {sendEmailCommand.EmailTemplateType.ToString()} to {sendEmailCommand.To}");

            sendEmailService.SendEmail(message);

            foreach (var recipient in sendEmailCommand.Recipients)
            {
                message.To = recipient;
                sendEmailService.SendEmail(message);
            }

            await Task.FromResult(context.Message);
        }



    }
}
