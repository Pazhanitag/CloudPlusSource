using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDistriputionGroupMember
{
    public class RemoveDistriputionGroupMemberActivity : IRemoveDistriputionGroupMemberActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public RemoveDistriputionGroupMemberActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveDistriputionGroupMemberArguments> context)
        {
            var args = context.Arguments;


            await _office365ApiService.RemoveDistriputionGroupMembersAsync(new Office365ApiDistributionGroupMembersModel
            {
                DistributionGroupName = args.DistributionGroupName,
                 MemberSMTPAddress=args.MemberSMTPAddress
            });

            return context.Completed();
        }

    }
}
