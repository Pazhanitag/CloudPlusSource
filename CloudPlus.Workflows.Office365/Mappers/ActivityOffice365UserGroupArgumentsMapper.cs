using System;
using CloudPlus.Models.Enums;
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

namespace CloudPlus.Workflows.Office365.Mappers
{
    public class ActivityOffice365UserGroupArgumentsMapper : IActivityOffice365UserGroupArgumentsMapper
    {
        public dynamic MapCreatePartnerPlatformAddDistributionGroupMember(ICreateDistributionGroupMemberArguments command)
        {
            var dest = new
            {
                command.DistributionGroupName,
                command.MemberSMTPAddress,
                WorkflowActivityType = WorkflowActivityType.Office365AddDistributionGroupMember,
                WorkflowStep = WorkflowActivityStep.Office365AddDistributionGroupMember
            };

            return dest;
        }

        public dynamic MapCreatePartnerPlatformAddO365GroupMember(ICreateO365GroupMemberArguments command)
        {
            var dest = new
            {
                command.DistributionGroupName,
                command.MemberSMTPAddress,
                WorkflowActivityType = WorkflowActivityType.Office365AddO365GroupMember,
                WorkflowStep = WorkflowActivityStep.Office365AddO365GroupMember
            };

            return dest;
        }

        public dynamic MapCreatePartnerPlatformAddSecurityGroupMember(ICreateSecurityGroupMemberArguments command)
        {
            var dest = new
            {
                command.CustomerO365Domain,
                //command.SecurityGroupName,
                command.UserSMTPAddress,
                command.Office365CustomerId,
                WorkflowActivityType = WorkflowActivityType.Office365AddSecurityGroupMember,
                WorkflowStep = WorkflowActivityStep.Office365AddSecurityGroupMember
            };

            return dest;
        }

        public dynamic MapCreatePartnerPlatformCreateDistributionGroup(ICreateDistributionGroupArguments command)
        {
            var dest = new
            {
                command.DistributionGroupName,
                command.CurrentUserPrincipalName,
                command.AssignToUserPrincipalName,
                command.MemberJoinPolicy,
                WorkflowActivityType = WorkflowActivityType.Office365CreateDistributionGroup,
                WorkflowStep = WorkflowActivityStep.Office365CreateDistributionGroup
            };

            return dest;
        }

        public dynamic MapCreatePartnerPlatformCreateO365Group(ICreateO365GroupArguments command)
        {
            var dest = new
            {
                command.DistributionGroupName,
                command.CurrentUserPrincipalName,
                command.AssignToUserPrincipalName,
                command.MemberJoinPolicy,
                WorkflowActivityType = WorkflowActivityType.Office365CreateO365Group,
                WorkflowStep = WorkflowActivityStep.Office365CreateO365Group
            };

            return dest;
        }

        public dynamic MapCreatePartnerPlatformCreateSecurityGroup(ICreateSecurityGroupArguments command)
        {
            var dest = new
            {
                command.CustomerO365Domain,
                command.SecurityGroupName,
                WorkflowActivityType = WorkflowActivityType.Office365CreateSecurityGroup,
                WorkflowStep = WorkflowActivityStep.Office365CreateSecurityGroup
            };

            return dest;
        }

        public dynamic MapCreatePartnerPlatformRemoveDistributionGroup(IRemoveDistriputionGroupArguments command)
        {
            var dest = new
            {
                command.DistributionGroupName,
                WorkflowActivityType = WorkflowActivityType.Office365RemoveDistributionGroup,
                WorkflowStep = WorkflowActivityStep.Office365RemoveDistributionGroup
            };

            return dest;
        }

        public dynamic MapCreatePartnerPlatformRemoveDistributionGroupMember(IRemoveDistriputionGroupMemberArguments command)
        {
            var dest = new
            {
                command.DistributionGroupName,
                command.MemberSMTPAddress,
                WorkflowActivityType = WorkflowActivityType.Office365RemoveDistributionGroupMember,
                WorkflowStep = WorkflowActivityStep.Office365RemoveDistributionGroupMember
            };

            return dest;
        }

        public dynamic MapCreatePartnerPlatformRemoveO365Group(IRemoveO365GroupArguments command)
        {
            var dest = new
            {
                command.DistributionGroupName,
                WorkflowActivityType = WorkflowActivityType.Office365RemoveO365Group,
                WorkflowStep = WorkflowActivityStep.Office365RemoveO365Group
            };

            return dest;
        }

        public dynamic MapCreatePartnerPlatformRemoveO365GroupMember(IRemoveO365GroupMemberArguments command)
        {
            var dest = new
            {
                command.DistributionGroupName,
                command.MemberSMTPAddress,
                WorkflowActivityType = WorkflowActivityType.Office365RemoveO365GroupMember,
                WorkflowStep = WorkflowActivityStep.Office365RemoveO365GroupMember
            };

            return dest;
        }

        public dynamic MapCreatePartnerPlatformRemoveSecurityGroup(IRemoveSecurityGroupArguments command)
        {
            var dest = new
            {
                command.CustomerO365Domain,
                command.SecurityGroupName,
                WorkflowActivityType = WorkflowActivityType.Office365RemoveSecurityGroup,
                WorkflowStep = WorkflowActivityStep.Office365RemoveSecurityGroup
            };

            return dest;
        }

        public dynamic MapCreatePartnerPlatformRemoveSecurityGroupMember(IRemoveSecurityGroupMemberArguments command)
        {
            var dest = new
            {
                command.CustomerO365Domain,
                command.SecurityGroupName,
                WorkflowActivityType = WorkflowActivityType.Office365RemoveSecurityGroupMember,
                WorkflowStep = WorkflowActivityStep.Office365RemoveSecurityGroupMember
            };

            return dest;
        }
    }
}
