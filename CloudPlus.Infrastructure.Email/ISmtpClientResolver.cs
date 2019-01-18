using System.Net.Mail;

namespace CloudPlus.Infrastructure.Email
{
    public interface ISmtpClientResolver
    {
        SmtpClient GetSmtpClient();
    }
}