using CloudPlus.Models.Office365.UserGroup;
using CloudPlus.Services.Database.Office365.UserGroup;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDatabaseDistributionGroup
{
    public class RemoveDatabaseDistributionGroupActivity : IRemoveDatabaseDistributionGroupActivity
    {
        private readonly IOffice365DbUserGroupService _office365DbUserGroupService;
        public RemoveDatabaseDistributionGroupActivity(Office365DbUserGroupService office365DbUserGroupService)
        {
            _office365DbUserGroupService = office365DbUserGroupService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveDatabaseDistributionGroupArguments> context)
        {
            var args = context.Arguments;
            await _office365DbUserGroupService.RemoveOffice365DistributionGroupAsync(new Office365DistributionGroupModel
            {
                DistributionGroupName = args.DistributionGroupName,
                UserPrincipalName = args.UserSMTPAddress
            });


            return context.Completed();
        }
    }
}
