using System;
using Autofac;
using CloudPlus.Authentication.Identity.Factory;
using CloudPlus.Authentication.Identity.Roles;
using CloudPlus.Authentication.Identity.Users;
using IdentityServer3.Core.Configuration;
using Owin;
using System.Collections.Generic;
using System.Web.Http;
using CloudPlus.Database.Authentication;
using CloudPlus.Resources;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Serilog;

namespace CloudPlus.Authentication.Api.Extensions
{
    public static class IdentityServerConfig
    {
        public static void UseIdentityServerAuthFlow(this IAppBuilder app,
            HttpConfiguration httpConfig)
        {
            var configurationManager = IoC.GetContainer().Resolve<IConfigurationManager>();

            app.CreatePerOwinContext(() => new CloudPlusAuthDbContext());
            app.CreatePerOwinContext((IdentityFactoryOptions<IdentityUserManager> identityFactoryOptions, IOwinContext owin) => 
                new IdentityUserManager(owin, configurationManager));
            app.CreatePerOwinContext<IdentityRoleManager>(IdentityRoleManager.Create);

            var identityServerFactory = IoC.GetContainer().Resolve<IIdentityFactory>();

            var factory = identityServerFactory.Initialize(configurationManager.GetByKey("DbConnectionStringName"));

            bool.TryParse(configurationManager.GetByKey("ApiTraceLogging"), out bool apiTraceLogging);

            if (apiTraceLogging)
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Trace()
                    .CreateLogger();
            }

            var authOptions = new AuthenticationOptions
            {
                EnableSignOutPrompt = false,
                RequireSignOutPrompt = false,
                EnablePostSignOutAutoRedirect = true,
                PostSignOutAutoRedirectDelay = 0,
                
                LoginPageLinks = new List<LoginPageLink>
                {
                    new LoginPageLink
                    {
                        Href = configurationManager.GetByKey("ForgotPasswordEndpoint"),
                        Text = configurationManager.GetByKey("ForgotPasswordLinkText")
                    }
                }
            };

            var options = new IdentityServerOptions
            {
                SiteName = configurationManager.GetByKey("SiteName"),
                SigningCertificate = Certificate.Certificate.Load(),
                Factory = factory,
                RequireSsl = false,
                EnableWelcomePage = true,
                AuthenticationOptions = authOptions
            };

            app.UseIdentityServer(options);
        }
    }
}