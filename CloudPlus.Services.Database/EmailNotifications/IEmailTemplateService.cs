using CloudPlus.Models.Notification;

namespace CloudPlus.Services.Database.EmailNotifications
{
	public interface IEmailTemplateService
	{
	    EmailTemplateContentModel GetEmailTemplate(EmailTemplatePlaceholdersGeneratorModel data);
	}
}