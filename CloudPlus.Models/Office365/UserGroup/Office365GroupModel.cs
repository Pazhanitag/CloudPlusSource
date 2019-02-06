using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Models.Office365.UserGroup
{
    public class Office365GroupModel
    {
        public string Office365GroupId { get; set; }
        public string Office365GroupName { get; set; }
        public string UserPrincipalName { get; set; }
    }
}
