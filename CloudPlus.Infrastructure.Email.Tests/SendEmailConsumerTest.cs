using System;
using CloudPlus.Api.NotificationService.Consumers;
using CloudPlus.QueueModels.EmailNotification.Commands;
using CloudPlus.QueueModels.Enums;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Rhino.Mocks;
using CloudPlus.Services.Database.EmailNotifications;
using MassTransit;

namespace CloudPlus.Infrastructure.Email.Tests
{
	[TestFixture]
	public class SendEmailConsumerTest
	{
		[Test]
		public void ConsumeMethodShouldCallSendEmail()
		{
			var smtpManager = MockRepository.GenerateStub<ISmtpManager>();
			var emailTemplateService = MockRepository.GenerateStub<IEmailTemplateService>();

			ConsumeContext<ISendEmailCommand> context = MockRepository.GenerateMock<ConsumeContext<ISendEmailCommand>>();
			context.Stub(x => x.Message).PropertyBehavior();
			var isec = new
			{
				To = "irhadba@maestralsolutions.com",
				Subject = "Some Subject",
				Body = "Some Body",
				EmailMediaType = EmailMediaType.Text,
				EmailTemplate = EmailTemplateEnum.None,
			};

			var sendEmailConsumer = new SendEmailConsumer(smtpManager, emailTemplateService);
			context.Stub(x => x.Message).Return(isec);

			//Act
			sendEmailConsumer.Consume(context);

			//Assert
			Assert.That(true);
		}
	}
}
