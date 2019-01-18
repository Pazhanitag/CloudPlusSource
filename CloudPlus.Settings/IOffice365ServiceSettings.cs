namespace CloudPlus.Settings
{
    public interface IOffice365ServiceSettings : IRabbitMqSettings
    {
        string ApplicationId { get; }
        string ApplicationSecret { get; }
        string Domain { get; }
        string AuthenticationAuthorityEndpoint { get; }
        string GraphEndpoint { get; }
        string CommonDomain { get; }
        string UserName { get; }
        string Password { get; }
        string ResourceUrl { get; }
        string CloudPlusSupportEmail { get; }
        string CloudPlusEngineeringEmail { get; }
    }
}
