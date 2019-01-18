using Newtonsoft.Json;

namespace CloudPlus.Resources
{
    public class JsonSerializer : IObjectSerializer
    {
        public string Serialize<T>(T value)
        {
            if (ReferenceEquals(value, null))
                return string.Empty;

            try
            {
                return JsonConvert.SerializeObject(value, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include
                });
            }
            catch
            {
                return string.Empty;
            }
        }

        public T Deserialize<T>(string value)
        {
            if (value == null)
                return default(T);

            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch
            {
                return default(T);
            }
        }

        public object Deserialize(string value)
        {
            if (value == null)
                return default(object);

            try
            {
                return JsonConvert.DeserializeObject(value);
            }
            catch
            {
                return default(object);
            }
        }
    }
}