using CloudPlus.Infrastructure.Email;
using CloudPlus.Models.Notification;

namespace CloudPlus.AppServices.Notification.Services
{
    public class SendEmailService : ISendEmailService
    {
        private readonly ISmtpManager _smtpManager;

        public SendEmailService(ISmtpManager smtpManager)
        {
            _smtpManager = smtpManager;
        }

        public void SendEmail(EmailMessageModel message)
        {
            _smtpManager.SendEmail(message.To, message.Subject, message.Body, message.EmailMediaType, message.attachment, message.BCC);
        }
    }
}
