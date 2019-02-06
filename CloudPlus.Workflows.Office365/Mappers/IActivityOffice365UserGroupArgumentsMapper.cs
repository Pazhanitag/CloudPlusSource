using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDistriputionGroup;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDistriputionGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateO365Group;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateO365GroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateSecurityGroup;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateSecurityGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDistriputionGroup;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDistriputionGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveO365Group;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveO365GroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveSecurityGroup;
using CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveSecurityGroupMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Mappers
{
    public interface IActivityOffice365UserGroupArgumentsMapper
    {
        dynamic MapCreatePartnerPlatformCreateDistributionGroup(ICreateDistributionGroupArguments src);
        dynamic MapCreatePartnerPlatformCreateSecurityGroup(ICreateSecurityGroupArguments src);
        dynamic MapCreatePartnerPlatformCreateO365Group(ICreateO365GroupArguments src);

        dynamic MapCreatePartnerPlatformAddO365GroupMember(ICreateO365GroupMemberArguments src);
        dynamic MapCreatePartnerPlatformAddDistributionGroupMember(ICreateDistributionGroupMemberArguments src);
        dynamic MapCreatePartnerPlatformAddSecurityGroupMember(ICreateSecurityGroupMemberArguments src);

        dynamic MapCreatePartnerPlatformRemoveDistributionGroup(IRemoveDistriputionGroupArguments src);
        dynamic MapCreatePartnerPlatformRemoveSecurityGroup(IRemoveSecurityGroupArguments src);
        dynamic MapCreatePartnerPlatformRemoveO365Group(IRemoveO365GroupArguments src);

        dynamic MapCreatePartnerPlatformRemoveO365GroupMember(IRemoveO365GroupMemberArguments src);
        dynamic MapCreatePartnerPlatformRemoveDistributionGroupMember(IRemoveDistriputionGroupMemberArguments src);
        dynamic MapCreatePartnerPlatformRemoveSecurityGroupMember(IRemoveSecurityGroupMemberArguments src);

        
    }
}
