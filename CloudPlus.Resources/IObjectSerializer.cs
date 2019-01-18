namespace CloudPlus.Resources
{
    public interface IObjectSerializer
    {
        string Serialize<T>(T value);
        T Deserialize<T>(string value);
        object Deserialize(string value);
    }
}