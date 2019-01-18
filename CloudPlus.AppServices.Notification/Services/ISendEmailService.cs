using CloudPlus.Models.Notification;

namespace CloudPlus.AppServices.Notification.Services
{
    public interface ISendEmailService
    {
        void SendEmail(EmailMessageModel message);

    }
}
