using CloudPlus.Models.Notification;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CloudPlus.AppServices.Notification.Services
{
    public interface IEmailMessageService
    {
        EmailTemplateContentModel GetEmailTemplate(EmailTemplatePlaceholdersGeneratorRequestModel data);
        Task<Attachment> SendProductDetailsAsEmail(int companyId, int catalogId);
    }
}
