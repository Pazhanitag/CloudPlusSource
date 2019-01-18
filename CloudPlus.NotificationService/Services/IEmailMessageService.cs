using CloudPlus.Enums.Notification;
using CloudPlus.Models.Notification;

namespace CloudPlus.Api.NotificationService.Services
{
    public interface IEmailMessageService
    {
        EmailTemplateContentModel GetWelcomeUserPasswordViaEmailTemplate(EmailTemplateType type,
            string userEmail, string tempLink);

        EmailTemplateContentModel GetWelcomeUserSendPlainPasswordViaEmailTemplate(EmailTemplateType type,
            string userName, string password);

        EmailTemplateContentModel GetWelcomeUserDontSendPasswordTemplate(EmailTemplateType type,
            string userName);

        EmailTemplateContentModel GetForgotPasswordTemplate(EmailTemplateType type,
            string userName, string tempResetLink);

        EmailTemplateContentModel GetWelcomeCompanyCustomerTemplate(EmailTemplateType type,
            string userName, string tempResetLink, int companyId);

        EmailTemplateContentModel GetWelcomeCompanyResellerTemplate(EmailTemplateType type,
            string userName, string tempResetLink, int companyId);

        EmailTemplateContentModel GetChangePasswordTemplate(EmailTemplateType emailTemplateType, 
            string userName, string tempResetLink);

        EmailTemplateContentModel GetPasswordChangedTemplate(EmailTemplateType emailTemplateType, 
            string userName);

        EmailTemplateContentModel GetPasswordChangedSendPlainPasswordViaEmailTemplate(EmailTemplateType emailTemplateType, 
            string userName, string password);
    }
}
