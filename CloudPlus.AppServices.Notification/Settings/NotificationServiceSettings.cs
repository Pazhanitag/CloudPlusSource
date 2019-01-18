using CloudPlus.Resources;
using CloudPlus.Settings;

namespace CloudPlus.AppServices.Notification.Settings
{
    public class NotificationServiceSettings : INotificationServiceSettings
    {
        private readonly IConfigurationManager _configurationManager;

        public NotificationServiceSettings(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public string RabbitMqUri => _configurationManager.GetByKey("RabbitMqUri");
        public string RabbitMqUserName => _configurationManager.GetByKey("RabbitMqUsername");
        public string RabbitMqPassword => _configurationManager.GetByKey("RabbitMqPassword");
        public string ARecordIp => _configurationManager.GetByKey("ARecordIp");
    }
}
