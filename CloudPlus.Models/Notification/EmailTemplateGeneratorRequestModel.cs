using System.Collections.Generic;
using CloudPlus.Enums.Notification;

namespace CloudPlus.Models.Notification
{
    public class EmailTemplatePlaceholdersGeneratorModel
    {
        public EmailTemplateType EmailTemplateType { get; set; }
        public Dictionary<string, string> PlacehoderList { get; set; }
    }
}
