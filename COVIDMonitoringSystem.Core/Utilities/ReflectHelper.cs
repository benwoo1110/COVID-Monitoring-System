//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace COVIDMonitoringSystem.Core.Utilities
{
    public static class ReflectHelper
    {
        public static Dictionary<string, TF> GetFieldsOfType<TF>(object obj) where TF : class
        {
            var fieldValues = new Dictionary<string, TF>();
            var fieldInfos = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var fieldInfo in fieldInfos)
            {
                var value = fieldInfo.GetValue(obj);
                if (value is TF targetValue)
                {
                    fieldValues.Add(fieldInfo.Name, targetValue);
                }
            }

            return fieldValues;
        }

        public static Dictionary<MethodInfo, TA> GetMethodWithAttribute<TA>(object obj) where TA : Attribute
        {
            var methodAttributeMap = new Dictionary<MethodInfo, TA>();
            var methodInfos = obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var methodInfo in methodInfos)
            {
                var attribute = methodInfo.GetCustomAttribute(typeof(TA), true);
                if (attribute is TA targetAttribute)
                {
                    methodAttributeMap.Add(methodInfo, targetAttribute);
                }   
            }

            return methodAttributeMap;
        }

        public static List<KeyValuePair<ParameterInfo, TA>> GetParametersAttributeList<TA>(MethodInfo methodInfo) where TA : Attribute
        {
            var parameterAttributeMap = new List<KeyValuePair<ParameterInfo, TA>>();
            var parameterInfos = methodInfo.GetParameters();

            foreach (var parameter in parameterInfos)
            {
                var attribute = parameter.GetCustomAttribute(typeof(TA), true);
                if (attribute is TA targetAttribute)
                {
                    parameterAttributeMap.Add(new KeyValuePair<ParameterInfo, TA>(parameter, targetAttribute));
                    continue;
                }

                parameterAttributeMap.Add(new KeyValuePair<ParameterInfo, TA>(parameter, null));
            }

            return parameterAttributeMap;
        }

        public static Dictionary<ParameterInfo, TA> GetParametersAttributeDict<TA>(MethodInfo methodInfo)
            where TA : Attribute
        {
            var parameterAttributeMap = new Dictionary<ParameterInfo, TA>();
            var parameterInfos = methodInfo.GetParameters();

            foreach (var parameter in parameterInfos)
            {
                var attribute = parameter.GetCustomAttribute(typeof(TA), true);
                if (attribute is TA targetAttribute)
                {
                    parameterAttributeMap.Add(parameter, targetAttribute);
                    continue;
                }

                parameterAttributeMap.Add(parameter, null);
            }

            return parameterAttributeMap;
        }
        
        public static Dictionary<string, string> GetAllPropertyValues<T>(T obj) where T : class
        {
            var propertyValues = new Dictionary<string, string>();
            GetAllPropertyValues(propertyValues, obj);
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

        public static string GetTypeName<T>(T obj)
        {
            return obj.GetType().Name;
        }
    }
}