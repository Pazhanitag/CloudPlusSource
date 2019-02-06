using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.QueueModels.Office365.Subscriptions.Commands
{
    public interface IManageMultiSubscriptionsAndLicencesCommand
    {
        int CompanyId { get; set; }
        IEnumerable<string> CloudPlusProductIdentifier { get; set; }
        IEnumerable<IOffice365LicenceUser> Users { get; set; }
        IEnumerable<string> UserRoles { get; set; }
        IEnumerable<string> AddOn { get; set; }
        IEnumerable<string> SecurityGroupName { get; set; }
        IEnumerable<string> DistributionGroupName { get; set; }
        IEnumerable<string> Office365GroupName { get; set; }
    }
}
