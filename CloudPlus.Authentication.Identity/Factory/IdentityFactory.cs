using CloudPlus.Authentication.Identity.Roles;
using CloudPlus.Authentication.Identity.Services;
using CloudPlus.Authentication.Identity.Users;
using CloudPlus.Database.Authentication;
using CloudPlus.Infrastructure.Http;
using CloudPlus.Resources;
using CloudPlus.Services.Identity.Permission;
using CloudPlus.Services.Identity.User;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.EntityFramework;
using TokenProviderService = CloudPlus.Authentication.Identity.Services.TokenProviderService;
using ITokenProviderService = CloudPlus.Authentication.Identity.Services.ITokenProviderService;
using IUserService = IdentityServer3.Core.Services.IUserService;

namespace CloudPlus.Authentication.Identity.Factory
{
    public class IdentityFactory : IIdentityFactory
    {
        private readonly IConfigurationManager _configurationManager;

        public IdentityFactory(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public IdentityServerServiceFactory Initialize(string connectionStringName)
        {
            var defaultViewServiceOptions = new DefaultViewServiceOptions();
            defaultViewServiceOptions.Stylesheets.Add(_configurationManager.GetByKey("Assets.bulma.css"));
            defaultViewServiceOptions.Stylesheets.Add(_configurationManager.GetByKey("Assets.error.css"));
            defaultViewServiceOptions.Stylesheets.Add(_configurationManager.GetByKey("Assets.forgotPassword.css"));
            defaultViewServiceOptions.Stylesheets.Add(_configurationManager.GetByKey("Assets.login.css"));
            defaultViewServiceOptions.CacheViews = false;

            var factory = new IdentityServerServiceFactory();

            factory.ConfigureDefaultViewService(defaultViewServiceOptions);

            var entityFrameworkOptions = new EntityFrameworkServiceOptions
            {
                ConnectionString = connectionStringName
            };

            factory.RegisterConfigurationServices(entityFrameworkOptions);
            factory.RegisterOperationalServices(entityFrameworkOptions);

            factory.Register(new Registration<CloudPlusAuthDbContext>());
            factory.Register(new Registration<UserStore>());
            factory.Register(new Registration<RoleStore>());
            factory.Register(new Registration<IdentityUserManager>());
            factory.Register(new Registration<IdentityRoleManager>());
            factory.Register(new Registration<IConfigurationManager, ConfigurationManager>());
            factory.Register(new Registration<IImpersonateUserService, ImpersonateUserService>());
            factory.Register(new Registration<IHttpClientResolver, HttpClientResolver>());
            factory.Register(new Registration<IPermissionService, PermissionService>());
            factory.Register(new Registration<CloudPlus.Services.Identity.User.IUserService, UserService>());

            factory.Register(new Registration<ITokenProviderService>(x => 
                new TokenProviderService(x.Resolve<IdentityUserManager>(),
                    x.Resolve<CloudPlusAuthDbContext>())));

            factory.UserService = new Registration<IUserService>(resolver =>
                new IdentityUserService(
                    resolver.Resolve<IdentityUserManager>(),
                    resolver.Resolve<IImpersonateUserService>(),
                    resolver.Resolve<IConfigurationManager>()));

            factory.ClaimsProvider = new Registration<IClaimsProvider>(typeof(IdentityClaimsProvider));

            factory.CorsPolicyService =
                new Registration<ICorsPolicyService>(new DefaultCorsPolicyService
                {
                    AllowAll = true
                });

            return factory;
        }
    }
}
