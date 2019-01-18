namespace CloudPlus.Settings
{
    public interface INotificationServiceSettings : IRabbitMqSettings
    {
        string ARecordIp { get; }
    }
}
