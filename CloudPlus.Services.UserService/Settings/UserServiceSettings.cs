using CloudPlus.Resources;
using CloudPlus.Settings;

namespace CloudPlus.AppServices.User.Settings
{
    public class UserServiceSettings : IUserServiceSettings
    {
        private readonly IConfigurationManager _configurationManager;

        public UserServiceSettings(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        // RabbitMq
        public string RabbitMqUri => _configurationManager.GetByKey("RabbitMqUri");
        public string RabbitMqUserName => _configurationManager.GetByKey("RabbitMqUsername");
        public string RabbitMqPassword => _configurationManager.GetByKey("RabbitMqPassword");
        //TAG Dev: Added to send mail with BCC
        public string CloudPlusSupportEmail => _configurationManager.GetByKey("CloudPlusSupportEmail");
        public string CloudPlusEngineeringEmail => _configurationManager.GetByKey("CloudPlusEngineeringEmail");
    }
}
