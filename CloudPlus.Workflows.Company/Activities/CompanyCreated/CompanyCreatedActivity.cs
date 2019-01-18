using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Constants;
using CloudPlus.Enums.Notification;
using CloudPlus.Enums.User;
using CloudPlus.Logging;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.EmailNotification.Commands;
using CloudPlus.Services.Identity.Password;
using System.Collections.Generic;

namespace CloudPlus.Workflows.Company.Activities.CompanyCreated
{
    public class CompanyCreatedActivity : ICompanyCreatedActivity
    {
        private readonly IPasswordService _passwordService;

        public CompanyCreatedActivity(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ICompanyCreatedArguments> context)
        {
            try
            {
                var sendEndpoint = await context.GetSendEndpoint(NotificationServiceConstants.SendEmailNotificationUri);
                var args = context.Arguments;
                if (args.SendWelcomeLetters)
                {
                    if (args.Company.Type == CompanyType.Customer)
                    {
                        if (args.PasswordSetupMethod == PasswordSetupMethod.GeneratePasswordViaLink)
                        {
                            var tempResetLink = await _passwordService.GetPasswordResetLink(args.UserId, args.Email);

                            await sendEndpoint.Send<ISendEmailCommand>(
                                new
                                {
                                    UserName = args.Email,
                                    TempResetLink = tempResetLink,
                                    To = args.PasswordSetupEmail,
                                    Recipients = args.PasswordSetupEmail==args.Email ? null : new List<string>( new string[] { args.Email }  ),
                                    EmailTemplateType = EmailTemplateType.WelcomeCompanyCustomerPasswordViaEmail
                                });
                        }
                        else
                        {
                            if (args.SendPlainPasswordViaEmail)
                            {
                                await sendEndpoint.Send<ISendEmailCommand>(
                                    new
                                    {
                                        UserName = args.Email,
                                        args.Password,
                                        To = args.PasswordSetupEmail,
                                        EmailTemplateType = EmailTemplateType.WelcomeCompanyCustomerSendPlainPasswordViaEmail
                                    });
                            }
                            else
                            {
                                var userSendEmail = string.IsNullOrWhiteSpace(args.AlternativeEmail) ? args.Company.Email : args.AlternativeEmail;


                                await sendEndpoint.Send<ISendEmailCommand>(
                                   new
                                   {
                                      UserName = args.Email,
                                       args.Password,
                                       To = userSendEmail,
                                      EmailTemplateType = EmailTemplateType.WelcomeCompanyCustomer
                                   });                                
                            }
                        }
                    }
                    else
                    {
                        if (args.PasswordSetupMethod == PasswordSetupMethod.GeneratePasswordViaLink)
                        {
                            var tempResetLink = await _passwordService.GetPasswordResetLink(args.UserId, args.Email);

                            await sendEndpoint.Send<ISendEmailCommand>(
                                new
                                {
                                    UserName = args.Email,
                                    TempResetLink = tempResetLink,
                                    To = args.PasswordSetupEmail,
                                    Recipients = args.PasswordSetupEmail == args.Email ? null : new List<string>(new string[] { args.Email }),
                                    EmailTemplateType = EmailTemplateType.WelcomeCompanyResellerPasswordViaEmail
                                });

                        }
                        else
                        {
                            if (args.SendPlainPasswordViaEmail)
                            {
                                await sendEndpoint.Send<ISendEmailCommand>(
                                    new
                                    {
                                        UserName = args.Email,
                                        args.Password,
                                        To = args.PasswordSetupEmail,
                                        EmailTemplateType = EmailTemplateType.WelcomeCompanyResellerSendPlainPasswordViaEmail
                                    });

                            }
                            else { 

                                var userSendEmail = string.IsNullOrWhiteSpace(args.AlternativeEmail) ? args.Company.Email : args.AlternativeEmail;
                       
                                await sendEndpoint.Send<ISendEmailCommand>(
                                     new
                                     {
                                        UserName = args.Email,
                                        args.Password,
                                        To = userSendEmail,
                                        EmailTemplateType = EmailTemplateType.WelcomeCompanyReseller
                                     });                          
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log().Error("Error executing CompanyCreatedActivity", ex);
            }

            return context.Completed();
        }
    }
}
