using CloudPlus.Models.Office365.UserGroup;
using CloudPlus.Services.Database.Office365.UserGroup;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseO365Group
{
    public class CreateDatabaseO365GroupActivity : ICreateDatabaseO365GroupActivity
    {
        private readonly IOffice365DbUserGroupService _office365DbUserGroupService;
        public CreateDatabaseO365GroupActivity(Office365DbUserGroupService office365DbUserGroupService)
        {
            _office365DbUserGroupService = office365DbUserGroupService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateDatabaseO365GroupArguments> context)
        {
            var args = context.Arguments;
            await _office365DbUserGroupService.CreateOffice365GroupAsync(new Office365GroupModel
            {
                Office365GroupName = args.Office365GroupName,
                UserPrincipalName = args.UserSMTPAddress
            });


            return context.Completed();
        }
    }
}
