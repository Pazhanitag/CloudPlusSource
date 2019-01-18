using System;
using System.Net;
using System.Net.Mail;
using CloudPlus.Resources;

namespace CloudPlus.Infrastructure.Email
{
    public class SmtpClientResolver : ISmtpClientResolver
    {
        private readonly IConfigurationManager _configurationManager;

        public SmtpClientResolver(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public SmtpClient GetSmtpClient()
        {
            var smtpClientHost = _configurationManager.GetByKey("SmtpClientHost");
            var smtpUsername = _configurationManager.GetByKey("SmtpUsername");
            var smtpPassword = _configurationManager.GetByKey("SmtpPassword");

            if (string.IsNullOrWhiteSpace(smtpClientHost) || string.IsNullOrWhiteSpace(smtpUsername) ||
                string.IsNullOrWhiteSpace(smtpPassword))
                throw new Exception("Invalid Smtp configuration");

            var smtpClient = new SmtpClient(smtpClientHost, Convert.ToInt32(587));
            var credentials = new NetworkCredential(smtpUsername, smtpPassword);
	        smtpClient.EnableSsl = Convert.ToBoolean(_configurationManager.GetByKey("SmtpEnableSSL"));

			if (Convert.ToBoolean(_configurationManager.GetByKey("SmtpAuthenticate"))) smtpClient.Credentials = credentials;

            return smtpClient;
        }
    }
}