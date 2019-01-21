using CloudPlus.Enums.Provisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Models.Provisions
{
    public class AssignedServicesModel
    {
        public string offerName { get; set; }
        public string cloudPlusProductIdentifier { get; set; }
        public UserProvisioningStatus Status { get; set; }
        public string StatusToDisplay
        {
            get
            {
                return Status.ToString();
            }
        }
    }
}
