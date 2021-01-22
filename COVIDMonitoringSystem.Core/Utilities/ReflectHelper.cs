using System;
using System.Collections;
using System.Collections.Generic;

namespace COVIDMonitoringSystem.Core.Utilities
{
    public static class ReflectHelper
    {
        public static Dictionary<string, string> GetAllPropertyValues<T>(T obj) where T : class
        {
            var propertyValues = new Dictionary<string, string>();
            GetAllPropertyValues<T>(propertyValues, obj);
            return propertyValues;
        }
        
        private static void GetAllPropertyValues<T>(IDictionary<string, string> propertyValues, T obj) where T : class
        {
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if (value == null)
                {
                    propertyValues.Add(property.Name, "NULL");   
                    continue;
                }
                if (IsCollection(property.PropertyType))
                {
                    propertyValues.Add(property.Name, ((ICollection) value).Count.ToString());
                    continue;
                }
                if (IsBuiltInType(property.PropertyType))
                {
                    propertyValues.Add(property.Name, value.ToString());
                    continue;
                }

                
                GetAllPropertyValues(propertyValues, value);
            }
        }

        public static bool IsBuiltInType(Type type)
        {
            return type.FullName != null && type.FullName.StartsWith("System");
        }

        public static bool IsCollection(Type type)
        {
            return typeof(ICollection).IsAssignableFrom(type);
        }
    }
}