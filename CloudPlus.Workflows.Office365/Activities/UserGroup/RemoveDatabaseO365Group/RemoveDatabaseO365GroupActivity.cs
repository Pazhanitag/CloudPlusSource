using CloudPlus.Models.Office365.UserGroup;
using CloudPlus.Services.Database.Office365.UserGroup;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDatabaseO365Group
{
    public class RemoveDatabaseO365GroupActivity : IRemoveDatabaseO365GroupActivity
    {
        private readonly IOffice365DbUserGroupService _office365DbUserGroupService;
        public RemoveDatabaseO365GroupActivity(Office365DbUserGroupService office365DbUserGroupService)
        {
            _office365DbUserGroupService = office365DbUserGroupService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveDatabaseO365GroupArguments> context)
        {
            var args = context.Arguments;
            await _office365DbUserGroupService.RemoveOffice365GroupAsync(new Office365GroupModel
            {
                Office365GroupName = args.Office365GroupName,
                UserPrincipalName = args.UserSMTPAddress
            });


            return context.Completed();
        }
    }
}
