using IdentityServer3.Core.Services.Default;

namespace CloudPlus.Authentication.Identity.ViewServices
{
    public class IdentityViewService : DefaultViewService
    {
        public IdentityViewService(DefaultViewServiceOptions config, IViewLoader viewLoader)
            : base(config, viewLoader)
        {
        }
    }
}