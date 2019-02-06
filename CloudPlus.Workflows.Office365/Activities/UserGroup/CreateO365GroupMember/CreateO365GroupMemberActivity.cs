using CloudPlus.Services.Database.Office365.Api;
using CloudPlus.Models.Office365.Api;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateO365GroupMember
{
    public class CreateO365GroupMemberActivity : ICreateO365GroupMemberActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public CreateO365GroupMemberActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateO365GroupMemberArguments> context)
        {
            var args = context.Arguments;


            await _office365ApiService.AddOffice365GroupMembersAsync(new Office365ApiDistributionGroupMembersModel
            {
                DistributionGroupName = args.DistributionGroupName,
                MemberSMTPAddress = args.MemberSMTPAddress
            });

            return context.Completed();
        }

    }
}
