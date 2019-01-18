using CloudPlus.Enums.Notification;
using CloudPlus.Models.Office365.Transition;

namespace CloudPlus.Models.Notification
{
    public class EmailTemplatePlaceholdersGeneratorRequestModel
    {
        public string UserName { get; set; }
        public int CompanyId { get; set; }
        public string Password { get; set; }
        public string TempResetLink { get; set; }
        public string Domain { get; set; }
        public string TxtRecord { get; set; }
        public int CustomSecurePanelId { get; set; }
        public Office365TransitionReportModel Report { get; set; }
        public EmailTemplateType EmailTemplateType { get; set; }
    }
}
