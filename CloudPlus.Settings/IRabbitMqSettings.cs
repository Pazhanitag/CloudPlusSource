namespace CloudPlus.Settings
{
    public interface IRabbitMqSettings
    {
        string RabbitMqUri { get; }
        string RabbitMqUserName { get; }
        string RabbitMqPassword { get; }
    }
}