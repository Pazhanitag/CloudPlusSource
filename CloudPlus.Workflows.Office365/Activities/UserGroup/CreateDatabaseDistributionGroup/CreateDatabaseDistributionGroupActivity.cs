using CloudPlus.Models.Office365.UserGroup;
using CloudPlus.Services.Database.Office365.UserGroup;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseDistributionGroup
{
    public class CreateDatabaseDistributionGroupActivity : ICreateDatabaseDistributionGroupActivity
    {
        private readonly IOffice365DbUserGroupService _office365DbUserGroupService;
        public CreateDatabaseDistributionGroupActivity(Office365DbUserGroupService office365DbUserGroupService)
        {
            _office365DbUserGroupService = office365DbUserGroupService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateDatabaseDistributionGroupArguments> context)
        {
            var args = context.Arguments;
            await _office365DbUserGroupService.CreateOffice365DistributionGroupAsync(new Office365DistributionGroupModel
            {
                DistributionGroupName = args.DistributionGroupName,
                UserPrincipalName = args.UserSMTPAddress
            });

            return context.Completed();
        }
    }
}
