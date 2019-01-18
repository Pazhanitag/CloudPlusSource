using CloudPlus.Entities;
using CloudPlus.Enums.Notification;
using System.Collections.Generic;
using System.Net.Mail;

namespace CloudPlus.Infrastructure.Email
{
    public interface ISmtpManager
    {
        void SendEmail(string emailAddressTo, string subject, string body, EmailMediaType mediaType, Attachment attachment, List<string> BCC);
    }

}