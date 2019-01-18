using CloudPlus.Resources;
using CloudPlus.Settings;

namespace CloudPlus.Api.Settings
{
    public class CloudPlusApiSettings : ICloudPlusApiSettings
    {
        private readonly IConfigurationManager _configurationManager;

        public CloudPlusApiSettings(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public string RabbitMqUri => _configurationManager.GetByKey("RabbitMqUri");
        public string RabbitMqUserName => _configurationManager.GetByKey("RabbitMqUsername");
        public string RabbitMqPassword => _configurationManager.GetByKey("RabbitMqPassword");
		public string GoogleRecaptchaSecretKey => _configurationManager.GetByKey("ReCaptcha.SecretKey");
	    public string GoogleRecaptchaAPIUri => _configurationManager.GetByKey("Recaptcha.Uri");
        public string CloudPlusSupportGroupEmail => _configurationManager.GetByKey("CloudPlusSupportGroupEmail");
    }
}
