using System;
using System.Threading.Tasks;
using CloudPlus.Constants;
using MassTransit.Courier;
using CloudPlus.QueueModels.EmailNotification.Commands;
using CloudPlus.Enums.Notification;
using CloudPlus.Logging;

namespace CloudPlus.Workflows.Office365.Activities.Domain.SendCustomerTxtRecords
{
    public class SendCustomerDomainTxtRecordsActivity : ISendCustomerDomainTxtRecordsActivity
    {
        public async Task<ExecutionResult> Execute(ExecuteContext<ISendCustomerDomainTxtRecordsArguments> context)
        {
            try
            {
                var arguments = context.Arguments;
                var sendEndpoint = await context.GetSendEndpoint(NotificationServiceConstants.SendEmailNotificationUri);

                await sendEndpoint.Send<ISendEmailCommand>(
                    new
                    {
                        arguments.CompanyId,
                        arguments.Domain,
                        To = arguments.Email,
                        TxtRecord = arguments.TxtRecords,
                        EmailTemplateType = EmailTemplateType.Office365CustomerServiceEnabled
                    });
            }
            catch (Exception ex)
            {
                this.Log().Error("Error occured sending Office365CustomerServiceEnabled email template!", ex);
            }

            return context.Completed();
        }
    }
}
