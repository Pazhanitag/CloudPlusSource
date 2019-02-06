using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDistriputionGroup
{
    public class CreateDistributionGroupActivity : ICreateDistributionGroupActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public CreateDistributionGroupActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateDistributionGroupArguments> context)
        {
            var args = context.Arguments;


            await _office365ApiService.CreateDistriputionGroupAsync(new Office365ApiDistributionGroupModel
            {
                DistributionGroupName = args.DistributionGroupName,
                ManagerSMTPAddress = args.CurrentUserPrincipalName,
                GroupSMTPAddress = args.AssignToUserPrincipalName,
                MemberJoinPolicy = "Open"
            });

            return context.Completed();
        }

    }
}
