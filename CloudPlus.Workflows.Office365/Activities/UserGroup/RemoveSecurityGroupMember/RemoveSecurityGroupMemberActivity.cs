using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveSecurityGroupMember
{
   public class RemoveSecurityGroupMemberActivity : IRemoveSecurityGroupMemberActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public RemoveSecurityGroupMemberActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveSecurityGroupMemberArguments> context)
        {
            var args = context.Arguments;


            await _office365ApiService.RemoveSecurityGroupMembersAsync(new Office365ApiSecurityGroupMemberModel
            {
                CustomerO365Domain = args.CustomerO365Domain,
                SecurityGroupName = args.SecurityGroupName
            });

            return context.Completed();
        }
    }
}
