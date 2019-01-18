using CloudPlus.Models.Notification;

namespace CloudPlus.Api.NotificationService.Services
{
    public interface ISendEmailService
    {
        void SendEmail(EmailMessageModel message);
    }
}
