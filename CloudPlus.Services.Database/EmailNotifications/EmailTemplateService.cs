using System;
using System.Collections.Generic;
using System.Linq;
using CloudPlus.Database;
using CloudPlus.Models.Notification;

namespace CloudPlus.Services.Database.EmailNotifications
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly CldpDbContext _dbContext;

        public EmailTemplateService(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // TODO Write and Update tests
        public EmailTemplateContentModel GetEmailTemplate(EmailTemplatePlaceholdersGeneratorModel data)
        {
            var dbTemplate = _dbContext.EmailTemplates.AsNoTracking().FirstOrDefault(t => t.Type == data.EmailTemplateType.ToString());

            if (dbTemplate == null)
                throw new Exception($"Can not find template {data.EmailTemplateType.ToString()}");

            var template = new EmailTemplateContentModel
            {
                Body = data.PlacehoderList != null ? PopulatePlaceholdersWithData(dbTemplate.Template, data.PlacehoderList) : dbTemplate.Template,
                Subject = data.PlacehoderList != null ? PopulatePlaceholdersWithData(dbTemplate.Subject, data.PlacehoderList) : dbTemplate.Subject,
                //Subject = dbTemplate.Subject
            };

            return template;
        }

        // TODO Write Tests
        private string PopulatePlaceholdersWithData(string template, Dictionary<string, string> placeholderList)
        {
            foreach (var placeholder in placeholderList)
            {
                if (string.IsNullOrWhiteSpace(placeholder.Value))
                    continue;

                template = template.Replace(placeholder.Key, placeholder.Value);
            }

            return template;
        }
    }
}
