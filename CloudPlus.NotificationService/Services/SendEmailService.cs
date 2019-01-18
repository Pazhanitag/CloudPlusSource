using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudPlus.Enums.Notification;
using CloudPlus.Infrastructure.Email;
using CloudPlus.Models.Notification;

namespace CloudPlus.Api.NotificationService.Services
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
            _smtpManager.SendEmail(message.To, message.Subject, message.Body, message.EmailMediaType);
        }
    }
}