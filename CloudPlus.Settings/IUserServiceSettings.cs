namespace CloudPlus.Settings
{
    public interface IUserServiceSettings : IRabbitMqSettings
    {
        string CloudPlusSupportEmail { get; }
        string CloudPlusEngineeringEmail { get; }
    }
}
