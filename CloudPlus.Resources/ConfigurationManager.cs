using System;

namespace CloudPlus.Resources
{
    public class ConfigurationManager : IConfigurationManager
    {
        public string GetByKey(string key)
        {
            var configurationValue = System.Configuration.ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(configurationValue))
                throw new NullReferenceException(nameof(configurationValue));

            return configurationValue;
        }

        public string GetConnectionString(string key)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
    }
}