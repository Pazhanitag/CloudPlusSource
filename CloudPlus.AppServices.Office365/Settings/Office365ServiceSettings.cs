using CloudPlus.Resources;
using CloudPlus.Settings;

namespace CloudPlus.AppServices.Office365.Settings
{
    public class Office365ServiceSettings : IOffice365ServiceSettings
    {
        private readonly IConfigurationManager _configurationManager;

        public Office365ServiceSettings(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public string ApplicationId => _configurationManager.GetByKey("Office365.ApplicationId");
        public string ApplicationSecret => _configurationManager.GetByKey("Office365.ApplicationSecret");
        public string Domain => _configurationManager.GetByKey("Office365.Domain");
        public string AuthenticationAuthorityEndpoint => _configurationManager.GetByKey("Office365.AuthenticationAuthorityEndpoint");
        public string GraphEndpoint => _configurationManager.GetByKey("Office365.GraphEndpoint");
        public string CommonDomain => _configurationManager.GetByKey("Office365.CommonDomain");
        public string UserName => _configurationManager.GetByKey("Office365.UserName");
        public string Password => _configurationManager.GetByKey("Office365.Password");
        public string ResourceUrl => _configurationManager.GetByKey("Office365.ResourceUrl");
        public string CloudPlusSupportEmail => _configurationManager.GetByKey("CloudPlusSupportEmail");
        public string CloudPlusEngineeringEmail => _configurationManager.GetByKey("CloudPlusEngineeringEmail");
        public string RabbitMqUri => _configurationManager.GetByKey("RabbitMqUri");
        public string RabbitMqUserName => _configurationManager.GetByKey("RabbitMqUsername");
        public string RabbitMqPassword => _configurationManager.GetByKey("RabbitMqPassword");
    }
}
