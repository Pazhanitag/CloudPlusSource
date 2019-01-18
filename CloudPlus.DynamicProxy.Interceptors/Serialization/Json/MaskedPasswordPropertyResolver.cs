using System;
using System.Collections.Generic;
using System.Linq;
using CloudPlus.DynamicProxy.Interceptors.Loggers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CloudPlus.DynamicProxy.Interceptors.Serialization.Json
{
    public class MaskedPasswordPropertyResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = base.CreateProperties(type, memberSerialization);

            foreach (var prop in props.Where(p => p.PropertyType == typeof(string)))
            {
                var pi = type.GetProperty(prop.UnderlyingName);
                if (pi != null && pi.Name.ToLower().Contains("password"))
                {
                    prop.ValueProvider = new MskedStringValueProvider();
                }
            }

            return props;
        }
    }
}