using System.Collections.Generic;
using System.Net.Mail;
using CloudPlus.Enums.Notification;
using CloudPlus.Models.Office365.Transition;

namespace CloudPlus.QueueModels.EmailNotification.Commands
{
    public interface ISendEmailCommand : IQueueModel
    {
        string UserName { get; set; }
        int CompanyId { get; set; }
        string TempResetLink { get; set; }
        string Password { get; set; }
        string Domain { get; set; }
        string TxtRecord { get; set; }
        Office365TransitionReportModel Report { get; set; }
        string To { get; set; }
        List<string> Recipients { get; set; }
        EmailTemplateType EmailTemplateType { get; set; }
        bool Attachment { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        int CatalogId { get; set; }
        //TAG Dev : added for send the bcc with mail 
        List<string> BCC { get; set; }
        int CustomSecureCompanyId { get; set; }
    }
}
