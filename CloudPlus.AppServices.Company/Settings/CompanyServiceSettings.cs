using CloudPlus.Resources;
using CloudPlus.Settings;

namespace CloudPlus.AppServices.Company.Settings
{
    public class CompanyServiceSettings : ICompanyServiceSettings
    {
        private readonly IConfigurationManager _configurationManager;

        public CompanyServiceSettings(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        // RabbitMq
        public string RabbitMqUri => _configurationManager.GetByKey("RabbitMqUri");
        public string RabbitMqUserName => _configurationManager.GetByKey("RabbitMqUsername");
        public string RabbitMqPassword => _configurationManager.GetByKey("RabbitMqPassword");
    }
}
