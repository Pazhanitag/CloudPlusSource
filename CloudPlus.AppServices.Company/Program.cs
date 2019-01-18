using Autofac;
using CloudPlus.AppServices.Company.Settings;
using Topshelf;
using Topshelf.Autofac;

namespace CloudPlus.AppServices.Company
{
    internal class Program
    {
        private static void Main()
        {
            HostFactory.Run(x =>
            {
                log4net.Config.XmlConfigurator.Configure();

                x.UseLog4Net();

                x.Service<CompanyProvisioningService>(s =>
                {
                    s.ConstructUsing(name => {
                        var container = IoC.SetupAutofacContainer();

                        return container.Resolve<CompanyProvisioningService>();
                    });
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("CloudPlus Company service for managing companies");
                x.SetServiceName("CloudPlus Company");
            });
        }
    }
}
