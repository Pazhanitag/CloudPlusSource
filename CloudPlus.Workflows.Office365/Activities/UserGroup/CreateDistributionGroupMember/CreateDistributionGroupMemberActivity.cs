using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDistriputionGroupMember
{
    public class CreateDistributionGroupMemberActivity : ICreateDistributionGroupMemberActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public CreateDistributionGroupMemberActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateDistributionGroupMemberArguments> context)
        {
            var args = context.Arguments;


            await _office365ApiService.AddDistriputionGroupMembersAsync(new Office365ApiDistributionGroupMembersModel
            {
                DistributionGroupName = args.DistributionGroupName,
                MemberSMTPAddress = args.MemberSMTPAddress
            });

            return context.Completed();
        }
    }
}
