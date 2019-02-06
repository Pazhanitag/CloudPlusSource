using CloudPlus.Models.Office365.UserGroup;
using CloudPlus.Services.Database.Office365.UserGroup;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDatabaseSecurityGroupMember
{
    public class RemoveDatabaseSecurityGroupMemberActivity : IRemoveDatabaseSecurityGroupMemberActivity
    {
        private readonly IOffice365DbUserGroupService _office365DbUserGroupService;
        public RemoveDatabaseSecurityGroupMemberActivity(Office365DbUserGroupService office365DbUserGroupService)
        {
            _office365DbUserGroupService = office365DbUserGroupService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveDatabaseSecurityGroupMemberArguments> context)
        {
            var args = context.Arguments;

            foreach (var user in args.Users)
            {
                List<Office365SecurityGroupMemberModel> SecurityGroup = await _office365DbUserGroupService.GetOffice365SecurityGroupUserAsync(user.UserPrincipalName);
                var InsertSecurityGroup = args.SecurityGroupName.Where(x => !SecurityGroup.Select(y => y.SecurityGroupName).Contains(x));
                foreach (var s in InsertSecurityGroup)
                {
                    await _office365DbUserGroupService.RemoveOffice365SecurityGroupUserAsync(new Office365SecurityGroupMemberModel
                    {
                        SecurityGroupName = s,
                        UserPrincipalName = user.UserPrincipalName
                    });
                }
            }


            return context.Completed();
        }
    }
}
