﻿using CloudPlus.Models.Office365.UserGroup;
using CloudPlus.Services.Database.Office365.UserGroup;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseSecurityGroup
{
    public class CreateDatabaseSecurityGroupActivity : ICreateDatabaseSecurityGroupActivity
    {
        private readonly IOffice365DbUserGroupService _office365DbUserGroupService;
        public CreateDatabaseSecurityGroupActivity(Office365DbUserGroupService office365DbUserGroupService)
        {
            _office365DbUserGroupService = office365DbUserGroupService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateDatabaseSecurityGroupArguments> context)
        {
            var args = context.Arguments;

            await _office365DbUserGroupService.CreateOffice365SecurityGroupAsync(new Office365SecurityGroupModel
            {
                SecurityGroupName = args.SecurityGroupName,
                UserPrincipalName = args.UserSMTPAddress
            });

            return context.Completed();
        }
    }
}
