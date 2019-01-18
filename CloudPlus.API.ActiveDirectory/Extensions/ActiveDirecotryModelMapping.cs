using System;
using System.Collections.Generic;
using System.Reflection;
using CloudPlus.Api.ActiveDirectory.Attributes;
using CloudPlus.Api.ActiveDirectory.Models;

namespace CloudPlus.Api.ActiveDirectory.Extensions
{
    public static class ActiveDirecotryModelMapping
    {
        public static Dictionary<string, object> MapPropertiesToActiveDirectoryParameters(this IActiveDirectoryModel model)
        {
            var dict = new Dictionary<string, object>();

            foreach (var propertyInfo in model.GetType().GetProperties())
            {
                var propertyValue = Convert.ToString(propertyInfo.GetValue(model));

                if (string.IsNullOrWhiteSpace(propertyValue)) continue;

                var activeDirecotoryProperty = propertyInfo.GetCustomAttribute<ActiveDirecotryName>();

                dict.Add(activeDirecotoryProperty != null ? activeDirecotoryProperty.AdPropertyName : propertyInfo.Name, propertyValue);
            }

            return dict;
        }
    }
}