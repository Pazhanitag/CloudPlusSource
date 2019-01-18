using Newtonsoft.Json.Serialization;

namespace CloudPlus.DynamicProxy.Interceptors.Serialization.Json
{
    public class MskedStringValueProvider : IValueProvider
    {
        public object GetValue(object target)
        {
            return "******";
        }
        public void SetValue(object target, object value)
        {
        }
    }
}