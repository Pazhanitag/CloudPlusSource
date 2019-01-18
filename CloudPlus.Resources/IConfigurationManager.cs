namespace CloudPlus.Resources
{
    public interface IConfigurationManager
    {
        string GetByKey(string key);
        string GetConnectionString(string key);
    }
}