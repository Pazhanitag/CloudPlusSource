using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudPlus.Constants;
using CloudPlus.Enums.Notification;
using CloudPlus.Logging;
using CloudPlus.QueueModels.EmailNotification.Commands;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Domain.SendDomainVerifiedEmail
{
    public class SendDomainVerifiedEmailActivity : ISendDomainVerifiedEmailActivity
    {
        public async Task<ExecutionResult> Execute(ExecuteContext<ISendDomainVerifiedEmailArguments> context)
        {
            try
            {
                var sendEndpoint = await context.GetSendEndpoint(NotificationServiceConstants.SendEmailNotificationUri);
                var arguments = context.Arguments;

                if (arguments.IsDomainPrimary)
                {
                    await sendEndpoint.Send<ISendEmailCommand>(
                        new
                        {
                            arguments.CompanyId,
                            To = arguments.Email,
                            arguments.Domain,
                            EmailTemplateType = EmailTemplateType.Office365PrimaryDomainVerifiedSetUp
                        });
                }
                else
                {
                    await sendEndpoint.Send<ISendEmailCommand>(
                        new
                        {
                            arguments.CompanyId,
                            To = arguments.Email,
                            arguments.Domain,
                            EmailTemplateType = EmailTemplateType.Office365AdditionalDomainVerified
                        });
                }
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured SendDomainVerifiedEmailActivity", ex);
            }

            return context.Completed();
        }
    }
}
