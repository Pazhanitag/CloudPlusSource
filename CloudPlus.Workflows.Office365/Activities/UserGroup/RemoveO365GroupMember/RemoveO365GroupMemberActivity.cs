using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveO365GroupMember
{
    public class RemoveO365GroupMemberActivity : IRemoveO365GroupMemberActivity
    {
      
        private readonly IOffice365ApiService _office365ApiService;

        public RemoveO365GroupMemberActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveO365GroupMemberArguments> context)
        {
            var args = context.Arguments;


            await _office365ApiService.RemoveOffice365GroupMembersAsync(new Office365ApiDistributionGroupMembersModel
            {
                DistributionGroupName = args.DistributionGroupName,
                MemberSMTPAddress = args.MemberSMTPAddress
            });

            return context.Completed();
        }
    }
}
