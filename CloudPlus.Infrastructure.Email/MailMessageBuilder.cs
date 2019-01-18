using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Infrastructure.Email
{
	public class MailMessageBuilder : IMailMessageBuilder
	{
		private MailAddressCollection _to;
		private MailAddress _from;
		private string _subject;
		private string _body;
		private bool _isBodyHtml;
		private List<AlternateView> _alternateViews;

		public MailMessageBuilder()
		{
			this._to = new MailAddressCollection();
			this._alternateViews = new List<AlternateView>();
		}

		public MailMessageBuilder AddTo(string toAddress)
		{
			this._to.Add(new MailAddress(toAddress));
			return this;
		}

		public MailMessageBuilder AddFrom(string fromAddress)
		{
			this._from = new MailAddress(fromAddress);
			return this;
		}

		public MailMessageBuilder AddSubject(string subject)
		{
			this._subject = subject;
			return this;
		}

		public MailMessageBuilder AddBody(string body)
		{
			this._body = body;
			return this;
		}

		public MailMessageBuilder IsBodyHtml(bool isHtml)
		{
			this._isBodyHtml = isHtml;
			return this;
		}

		public MailMessageBuilder AddAlternateView(AlternateView alternateView)
		{
			this._alternateViews.Add(alternateView);
			return this;
		}

		public MailMessage Build()
		{
			var theMailMessage = new MailMessage()
			{
				From = this._from,
				Subject = this._subject,
				Body = this._body,
				IsBodyHtml = this._isBodyHtml
			};

			foreach (var toAddress in this._to)
			{
				theMailMessage.To.Add(new MailAddress(toAddress.Address));
			}

			foreach (var alternateView in this._alternateViews)
			{
				theMailMessage.AlternateViews.Add(alternateView);
			}

			return theMailMessage;
		}
	}
}
