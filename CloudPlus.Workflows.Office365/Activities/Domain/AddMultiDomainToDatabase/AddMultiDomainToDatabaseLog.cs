using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Domain.AddMultiDomainToDatabase
{
    public class AddMultiDomainToDatabaseLog : IAddMultiDomainToDatabaseLog
    {
        public string Office365CustomerId { get; set; }
        public IEnumerable<string> Domains { get; set; }
    }
}
