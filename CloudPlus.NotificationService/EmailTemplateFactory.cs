using System;
using CloudPlus.Api.NotificationService.Services;
using CloudPlus.Enums.Notification;
using CloudPlus.Models.Notification;
using CloudPlus.QueueModels.EmailNotification.Commands;

namespace CloudPlus.Api.NotificationService
{
    public static class EmailTemplateFactory
    {
        public static EmailTemplateContentModel GetEmailTemplate(IEmailMessageService emailMessageService, ISendEmailUserCommand emailData)
        {
            EmailTemplateContentModel template;

            switch (emailData.EmailTemplateType)
            {
                case EmailTemplateType.WelcomeUserPasswordViaEmail:
                    template = emailMessageService.GetWelcomeUserPasswordViaEmailTemplate(emailData.EmailTemplateType, emailData.UserName, emailData.TempResetLink);
                    break;
                case EmailTemplateType.WelcomeUserSendPlainPasswordViaEmail:
                    template = emailMessageService.GetWelcomeUserSendPlainPasswordViaEmailTemplate(emailData.EmailTemplateType, emailData.UserName, emailData.Password);
                    break;
                case EmailTemplateType.WelcomeUserDontSendPassword:
                    template = emailMessageService.GetWelcomeUserDontSendPasswordTemplate(emailData.EmailTemplateType, emailData.UserName);
                    break;
                case EmailTemplateType.ForgotPassword:
                    template = emailMessageService.GetForgotPasswordTemplate(emailData.EmailTemplateType, emailData.UserName, emailData.TempResetLink);
                    break;
                case EmailTemplateType.WelcomeCompanyCustomer:
                    template = emailMessageService.GetWelcomeCompanyCustomerTemplate(emailData.EmailTemplateType, emailData.UserName, emailData.TempResetLink, emailData.CompanyId);
                    break;
                case EmailTemplateType.WelcomeCompanyReseller:
                    template = emailMessageService.GetWelcomeCompanyResellerTemplate(emailData.EmailTemplateType, emailData.UserName, emailData.TempResetLink, emailData.CompanyId);
                    break;
                case EmailTemplateType.ChangePassword:
                    template = emailMessageService.GetChangePasswordTemplate(emailData.EmailTemplateType, emailData.UserName, emailData.TempResetLink);
                    break;
                case EmailTemplateType.PasswordChanged:
                    template = emailMessageService.GetPasswordChangedTemplate(emailData.EmailTemplateType, emailData.UserName);
                    break;
                case EmailTemplateType.PasswordChangedSendPlainPasswordViaEmail:
                    template = emailMessageService.GetPasswordChangedSendPlainPasswordViaEmailTemplate(emailData.EmailTemplateType, emailData.UserName, emailData.Password);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return template;
        }
    }
}
