using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Domain.AddMultiDomainToDatabase
{
    public interface IAddMultiDomainToDatabaseLog
    {
        string Office365CustomerId { get; set; }
        IEnumerable<string> Domains { get; set; }
    }
}
