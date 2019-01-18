using Autofac;
using Topshelf;
using Topshelf.Autofac;
using CloudPlus.AppServices.Notification.Settings;

namespace CloudPlus.AppServices.Notification
{
    internal class Program
    {
        private static void Main()
        {
            HostFactory.Run(x =>
            {
                log4net.Config.XmlConfigurator.Configure();

                x.UseLog4Net();

                x.Service<NotificationProvisioningService>(s =>
                {
                    s.ConstructUsing(name =>
                    {
                        var container = IoC.SetupAutofacContainer();

                        return container.Resolve<NotificationProvisioningService>();
                    });
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("CloudPlus Notification service for user notification");
                x.SetServiceName("CloudPlus Notification");
            });
        }
    }
}
