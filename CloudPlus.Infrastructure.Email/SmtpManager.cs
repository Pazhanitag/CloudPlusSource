using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using CloudPlus.Entities;
using CloudPlus.Enums.Notification;
using CloudPlus.Resources;

namespace CloudPlus.Infrastructure.Email
{
    public class SmtpManager : ISmtpManager
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly ISmtpClientResolver _smtpClientResolver;

        public SmtpManager(IConfigurationManager configurationManager, ISmtpClientResolver smtpClientResolver)
        {
            _configurationManager = configurationManager;
            _smtpClientResolver = smtpClientResolver;
        }

        public void SendEmail(string emailAddressTo, string subject, string body, EmailMediaType mediaType, Attachment attachment, List<string> BCC)
        {
            if (string.IsNullOrWhiteSpace(emailAddressTo))
                throw new ArgumentNullException(nameof(emailAddressTo));

            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException(nameof(subject));

            if (string.IsNullOrWhiteSpace(body))
                throw new ArgumentNullException(nameof(body));

            var smtpAddressFrom = _configurationManager.GetByKey("SmtpAddressFrom");

            if (string.IsNullOrWhiteSpace(smtpAddressFrom))
                throw new Exception("Invalid Email Address");

            var smtpClient = _smtpClientResolver.GetSmtpClient();

            var mailMessageBuilder = new MailMessageBuilder();

            var mailMessage = mailMessageBuilder
                .AddFrom(smtpAddressFrom)
                .AddSubject(subject)
                .IsBodyHtml((mediaType != EmailMediaType.Text))
                .AddAlternateView(
                    AlternateView.CreateAlternateViewFromString(body, null,
                        mediaType == EmailMediaType.Text ? MediaTypeNames.Text.Plain : MediaTypeNames.Text.Html)
                ).Build();
            mailMessage.To.Add(emailAddressTo);
            if (BCC != null) {
                foreach (var Item in BCC)
                {
                    MailAddress addressBCC = new MailAddress(Item);
                    mailMessage.Bcc.Add(addressBCC);
                }
            }
           
            if (attachment != null)
            {
                mailMessage.Attachments.Add(attachment);
            }

            smtpClient.Send(mailMessage);
        }

    }
}