using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.QueueModels.Users.Commands
{
    public interface ICreateSecurityGroupCommand : IQueueModel
    {
        string SecurityGroupName { get; set; }
        string UserPrincipalName { get; set; }
    }
}
