﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateO365GroupMember
{
    public interface ICreateO365GroupMemberArguments
    {
        string DistributionGroupName { get; set; }
        string MemberSMTPAddress { get; set; }
    }
}
