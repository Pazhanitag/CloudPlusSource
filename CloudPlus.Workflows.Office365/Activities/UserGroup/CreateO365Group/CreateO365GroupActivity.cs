using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateO365Group
{
    public class CreateO365GroupActivity : ICreateO365GroupActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public CreateO365GroupActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateO365GroupArguments> context)
        {
            var args = context.Arguments;


            await _office365ApiService.CreateOffice365GroupAsync(new Office365ApiDistributionGroupModel
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
