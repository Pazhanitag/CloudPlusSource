using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDistriputionGroup
{
    public class RemoveDistriputionGroupActivity: IRemoveDistriputionGroupActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public RemoveDistriputionGroupActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveDistriputionGroupArguments> context)
        {
            var args = context.Arguments;


            await _office365ApiService.RemoveDistriputionGroupAsync(new Office365ApiRemoveDistributionGroupModel
            {
                 DistributionGroupName=args.DistributionGroupName
            });

            return context.Completed();
        }

    }
}
