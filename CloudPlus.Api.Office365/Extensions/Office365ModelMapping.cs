using System.Collections.Generic;
using System.Reflection;
using CloudPlus.Api.Office365.Attributes;
using CloudPlus.Api.Office365.Models;

namespace CloudPlus.Api.Office365.Extensions
{
    public static class Office365ModelMapping
    {
        public static Dictionary<string, object> MapPropertiesToOffice365Parameters(this IOffice365Model model)
        {
            var dict = new Dictionary<string, object>();

            foreach (var propertyInfo in model.GetType().GetProperties())
            {
                var propertyValue = propertyInfo.GetValue(model);

                if (propertyValue == null) continue;

                var office365Property = propertyInfo.GetCustomAttribute<Office365Name>();

                dict.Add(office365Property != null ? office365Property.O365PropertyName : propertyInfo.Name, propertyValue);
            }

            return dict;
        }
    }
}
