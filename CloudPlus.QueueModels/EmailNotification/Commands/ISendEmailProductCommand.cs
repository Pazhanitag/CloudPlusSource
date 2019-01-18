using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.QueueModels.EmailNotification.Commands
{
    public interface ISendEmailProductCommand : IQueueModel
    {
        string UserName { get; set; }
        string Subject { get; set; }
        int CompanyId { get; set; }
        string To { get; set; }
        List<string> Recipients { get; set; }
        bool attachemnet { get; set; }
        string Body { get; set; }
        int CatalogId { get; set; }
    }
}
