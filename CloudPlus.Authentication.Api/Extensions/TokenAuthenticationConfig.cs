using Autofac;
using CloudPlus.Resources;
using IdentityServer3.AccessTokenValidation;
using Owin;

namespace CloudPlus.Authentication.Api.Extensions
{
    public static class TokenAuthenticationConfig
    {
        public static IAppBuilder UseBearerTokenAuthentication(this IAppBuilder app)
        {
            var configurationManager = IoC.GetContainer().Resolve<IConfigurationManager>();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = configurationManager.GetByKey("CloudPlus.IdentityServerEndpoint"),
                DelayLoadMetadata = true,
                RequiredScopes = new[] { "openid", "roles", "profile", "email", "write" }
            });

            return app;
        }
    }
}