using IdentityServer3.Core.Configuration;

namespace CloudPlus.Authentication.Identity.Factory
{
    public interface IIdentityFactory
    {
        IdentityServerServiceFactory Initialize(string connectionStringName);
    }
}
