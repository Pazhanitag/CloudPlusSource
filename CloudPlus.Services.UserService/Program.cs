using Autofac;
using CloudPlus.AppServices.User.Settings;
using Topshelf;
using Topshelf.Autofac;

namespace CloudPlus.AppServices.User
{
    internal class Program
    {
        private static void Main()
        {
            HostFactory.Run(x =>
            {
                log4net.Config.XmlConfigurator.Configure();

                x.UseLog4Net();

                x.Service<UserProvisioningService>(s =>
                {
                    s.ConstructUsing(name => {
                        var container = IoC.SetupAutofacContainer();
                        return container.Resolve<UserProvisioningService>();
                    });
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("CloudPlus User service for managing users");
                x.SetServiceName("CloudPlus User");
            });
        }
    }
}