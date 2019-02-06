using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveO365Group
{
    public class RemoveO365GroupActivity : IRemoveO365GroupActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public RemoveO365GroupActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveO365GroupArguments> context)
        {
            var args = context.Arguments;


            await _office365ApiService.RemoveOffice365GroupAsync(new Office365ApiRemoveDistributionGroupModel
            {
                DistributionGroupName = args.DistributionGroupName
            });

            return context.Completed();
        }

    
}

}
