using Autofac;
using CloudPlus.AppServices.Office365.Settings;
using Topshelf;
using Topshelf.Autofac;

namespace CloudPlus.AppServices.Office365
{
    internal class Program
    {
        private static void Main()
        {
            HostFactory.Run(x =>
            {
                log4net.Config.XmlConfigurator.Configure();

                x.UseLog4Net();

                x.Service<Office365ProvisioningService>(s =>
                {
                    s.ConstructUsing(name => {
                        var container = IoC.SetupAutofacContainer();

                        return container.Resolve<Office365ProvisioningService>();
                        });
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("CloudPlus Office365 service for managing companies, users, subscriptions and licences");
                x.SetServiceName("CloudPlus Office365");
            });
        }
    }
}
