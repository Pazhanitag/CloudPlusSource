using Autofac;
using CloudPlus.Resources;
using IdentityServer3.AccessTokenValidation;
using Owin;

namespace CloudPlus.Api.NotificationService.Extensions
{
    public static class TokenAuthenticationConfig
    {
        public static void UseBearerTokenAuthentication(this IAppBuilder app)
        {
            var configurationManager = IoC.GetContainer().Resolve<IConfigurationManager>();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = configurationManager.GetByKey("CloudPlus.IdentityServerEndpoint"),
                DelayLoadMetadata = true,
                RequiredScopes = new[] { "read", "write" },
                TokenProvider = new QueryStringOAuthBearerProvider()
            });
        }
    }
}