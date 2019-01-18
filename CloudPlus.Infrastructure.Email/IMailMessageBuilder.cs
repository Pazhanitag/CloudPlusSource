using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Infrastructure.Email
{
	interface IMailMessageBuilder
	{
		MailMessageBuilder AddTo(string toAddress);

		MailMessageBuilder AddFrom(string fromAddress);

		MailMessageBuilder AddSubject(string subject);

		MailMessageBuilder AddBody(string body);

		MailMessageBuilder IsBodyHtml(bool isHtml);

		MailMessageBuilder AddAlternateView(AlternateView alternateView);

		MailMessage Build();
	}
}
