using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveSecurityGroup
{
    public class RemoveSecurityGroupActivity: IRemoveSecurityGroupActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public RemoveSecurityGroupActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveSecurityGroupArguments> context)
        {
            var args = context.Arguments;


            await _office365ApiService.RemoveSecurityGroupAsync(new Office365ApiSecurtyGroupModel
            {
                 CustomerO365Domain = args.CustomerO365Domain,
                  SecurityGroupName=args.SecurityGroupName
            });

            return context.Completed();
        }
    }
}
