using System.Collections.Generic;
using System.Net.Mail;
using CloudPlus.Enums.Notification;

namespace CloudPlus.Models.Notification
{
    public class EmailMessageModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Recipients { get; set; }
        public EmailMediaType EmailMediaType { get; set; }
        public EmailTemplateType EmailTemplateType { get; set; }
        public Attachment attachment { get; set; }
        public List<string> BCC { get; set; }
    }
}
