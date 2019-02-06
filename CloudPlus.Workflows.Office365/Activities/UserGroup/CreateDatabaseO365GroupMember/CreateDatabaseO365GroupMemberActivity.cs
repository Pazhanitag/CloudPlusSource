using CloudPlus.Models.Office365.UserGroup;
using CloudPlus.Services.Database.Office365.UserGroup;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseO365GroupMember
{
    public class CreateDatabaseO365GroupMemberActivity : ICreateDatabaseO365GroupMemberActivity
    {
        private readonly IOffice365DbUserGroupService _office365DbUserGroupService;
        public CreateDatabaseO365GroupMemberActivity(Office365DbUserGroupService office365DbUserGroupService)
        {
            _office365DbUserGroupService = office365DbUserGroupService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateDatabaseO365GroupMemberArguments> context)
        {
            var args = context.Arguments;

            foreach (var user in args.Users)
            {
                List<Office365GroupMemberModel> Office365Group = await _office365DbUserGroupService.GetOffice365GroupUserAsync(user.UserPrincipalName);
                var InsertOffice365Group = args.Office365GroupName.Where(x => !Office365Group.Select(y => y.Office365GroupName).Contains(x));
                foreach (var s in InsertOffice365Group)
                {
                    await _office365DbUserGroupService.CreateOffice365GroupUserAsync(new Office365GroupMemberModel
                    {
                        Office365GroupName = s,
                        UserPrincipalName = user.UserPrincipalName
                    });
                }
            }

            return context.Completed();
        }
    }
}
