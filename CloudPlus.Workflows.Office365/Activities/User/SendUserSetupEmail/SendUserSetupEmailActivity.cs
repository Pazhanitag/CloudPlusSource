using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Constants;
using CloudPlus.Enums.Notification;
using CloudPlus.Logging;
using CloudPlus.QueueModels.EmailNotification.Commands;
using System.Collections.Generic;
using CloudPlus.Settings;

namespace CloudPlus.Workflows.Office365.Activities.User.SendUserSetupEmail
{
    public class SendUserSetupEmailActivity : ISendUserSetupEmailActivity
    {
        private readonly IOffice365ServiceSettings _office365ServiceSettings;
        public SendUserSetupEmailActivity(
           IOffice365ServiceSettings office365ServiceSettings)
        {
            _office365ServiceSettings = office365ServiceSettings;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ISendUserSetupEmailArguments> context)
        {
            try
            {
                var sendEndpoint = await context.GetSendEndpoint(NotificationServiceConstants.SendEmailNotificationUri);
                var args = context.Arguments;
                var bcc = new List<string> { _office365ServiceSettings.CloudPlusSupportEmail,_office365ServiceSettings.CloudPlusEngineeringEmail };
                await sendEndpoint.Send<ISendEmailCommand>(
                    new
                    {
                        UserName = args.UserPrincipalName,
                        To = args.UserPrincipalName,
                        EmailTemplateType = EmailTemplateType.Office365UserSetUp,
                        BCC = bcc
                    });
            }
            catch (Exception ex)
            {
                this.Log().Error("Error executing SendUserSetupEmailActivity", ex);
            }

            return context.Completed();
        }
    }
}
