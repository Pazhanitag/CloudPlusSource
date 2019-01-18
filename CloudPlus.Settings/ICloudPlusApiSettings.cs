namespace CloudPlus.Settings
{
    public interface ICloudPlusApiSettings : IRabbitMqSettings
    {
	    string GoogleRecaptchaSecretKey { get; }
	    string GoogleRecaptchaAPIUri { get; }
        string CloudPlusSupportGroupEmail { get; }

    }
}
