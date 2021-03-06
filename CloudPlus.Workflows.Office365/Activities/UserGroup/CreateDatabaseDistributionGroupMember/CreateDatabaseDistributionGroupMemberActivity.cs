﻿using CloudPlus.Models.Office365.UserGroup;
using CloudPlus.Services.Database.Office365.UserGroup;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseDistributionGroupMember
{
    public class CreateDatabaseDistributionGroupMemberActivity : ICreateDatabaseDistributionGroupMemberActivity
    {
        private readonly IOffice365DbUserGroupService _office365DbUserGroupService;
        public CreateDatabaseDistributionGroupMemberActivity(Office365DbUserGroupService office365DbUserGroupService)
        {
            _office365DbUserGroupService = office365DbUserGroupService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateDatabaseDistributionGroupMemberArguments> context)
        {
            var args = context.Arguments;

            foreach (var user in args.Users)
            {
                List<Office365DistributionGroupMemberModel> DistributionGroup = await _office365DbUserGroupService.GetOffice365DistributionGroupUserAsync(user.UserPrincipalName);
                var InsertDistributionGroup = args.DistributionGroupName.Where(x => !DistributionGroup.Select(y => y.DistributionGroupName).Contains(x));
                foreach (var s in InsertDistributionGroup)
                {
                    await _office365DbUserGroupService.CreateOffice365DistributionGroupUserAsync(new Office365DistributionGroupMemberModel
                    {
                        DistributionGroupName = s,
                        UserPrincipalName = user.UserPrincipalName
                    });
                }
            }

            return context.Completed();
        }
    }
}
