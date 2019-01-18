using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Domain.AddMultiDomainToDatabase
{
    public interface IAddMultiDomainToDatabaseArguments
    {
        string Office365CustomerId { get; set; }
        IEnumerable<string> Domains { get; set; }
    }
}
