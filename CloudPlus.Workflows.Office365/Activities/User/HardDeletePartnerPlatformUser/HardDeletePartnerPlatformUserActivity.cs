using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.HardDeletePartnerPlatformUser
{
    public class HardDeletePartnerPlatformUserActivity : IHardDeletePartnerPlatformUserActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public HardDeletePartnerPlatformUserActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public Task<ExecutionResult> Execute(ExecuteContext<IHardDeletePartnerPlatformUserArguments> context)
        {
            var arguments = context.Arguments;

            try
            {
                _office365ApiService.UserHardDeleteAsync(new Office365ApiUserModel
                {
                    Office365CustomerId = arguments.Office365CustomerId,
                    UserPrincipalName = arguments.UserPrincipalName
                });
            }
            catch (System.Exception ex)
            {
                this.Log().Error(ex);

                if(!arguments.SwallowException) // [01/27/2018] Kljuco: I don't like this way of doing it but this is the quickest way. When federating domain we need this ex to be swallowed
                    throw;
            }
            return Task.FromResult(context.Completed());
        }
    }
}