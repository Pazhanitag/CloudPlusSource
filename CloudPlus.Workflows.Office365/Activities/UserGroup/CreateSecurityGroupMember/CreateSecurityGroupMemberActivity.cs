using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Models.Office365.UserGroup;
using CloudPlus.Services.Database.Office365.Api;
using CloudPlus.Services.Database.Office365.UserGroup;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateSecurityGroupMember
{
    public class CreateSecurityGroupMemberActivity : ICreateSecurityGroupMemberActivity
    {
        private readonly IOffice365ApiService _office365ApiService;
        private readonly IOffice365DbUserGroupService _office365DbUserGroupService;
        public CreateSecurityGroupMemberActivity(IOffice365ApiService office365ApiService, Office365DbUserGroupService office365DbUserGroupService)
        {
            _office365ApiService = office365ApiService;
            _office365DbUserGroupService = office365DbUserGroupService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateSecurityGroupMemberArguments> context)
        {
            var args = context.Arguments;

            foreach (var user in args.Users)
            {
                List<Office365SecurityGroupMemberModel> SecurityGroup = await _office365DbUserGroupService.GetOffice365SecurityGroupUserAsync(user.UserPrincipalName);
                var InsertSecurityGroup=args.SecurityGroupName.Where(x => !SecurityGroup.Select(y => y.SecurityGroupName).Contains(x));
                foreach (var s in InsertSecurityGroup)
                {
                    var Domain = user.UserPrincipalName.Split('@').ToList();
                    if (Domain.Count>1)
                    await _office365ApiService.AddSecurityGroupMembersAsync(new Office365ApiSecurityGroupMemberModel
                    {
                        CustomerO365Domain = Domain[1],
                        SecurityGroupName = s,
                        UserSMTPAddress = user.UserPrincipalName
                    });

                }
            }

            return context.Completed();
        }
    }
}