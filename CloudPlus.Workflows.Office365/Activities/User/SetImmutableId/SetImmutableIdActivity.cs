using System;
using System.Threading.Tasks;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.SetImmutableId
{
    public class SetImmutableIdActivity : ISetImmutableIdActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public SetImmutableIdActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<SetImmutableIdArguments> context)
        {
            var result = await _office365ApiService.SetImmutableId(new Office365ImmutableIdModel
            {
                Office365CustomerId = context.Arguments.Office365CustomerId,
                UserPrincipalName = context.Arguments.UserPrincipalName
            });
            
            if(!result)
                throw new Exception($"Could not set immutable id for user {context.Arguments.UserPrincipalName}");
            return context.Completed();
        }
    }
}