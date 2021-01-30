//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core.Utilities;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public class InputResolverManager
    {
        private Dictionary<Type, Func<AbstractScreen, string, object>> resolverMap;

        public InputResolverManager()
        {
            resolverMap = new Dictionary<Type, Func<AbstractScreen, string, object>>();
            
            RegisterInputResolver<string>((screen, value) =>
            {
                //TODO: better checking empty string.
                return value;
            });

            RegisterInputResolver<int>((screen, value) =>
            {
                //TODO: Result
                return Convert.ToInt32(value);
            });

            RegisterInputResolver<double>((screen, value) =>
            {
                //TODO: Result
                return Convert.ToDouble(value);
            });
            
            RegisterInputResolver<DateTime>((screen, value) =>
            {
                //TODO: Result
                return Convert.ToDateTime(value);
            });
        }

        public void RegisterQuickObjectResolver<T>(Func<string, T> converter, string errorMessage) where T : class
        {
            RegisterInputResolver<T>((screen, value) =>
            {
                T result;
                try
                {
                    result = converter(value);
                    if (result == null)
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    throw new InputParseFailedException(string.Format(errorMessage, value));
                }

                return result;
            });
        }

        public void RegisterQuickEnumResolver<TE>(string enumName = null) where TE : struct
        {
            if (string.IsNullOrEmpty(enumName))
            {
                enumName = typeof(TE).Name;
            }
            
            RegisterInputResolver<TE>((screen, value) =>
            {
                try
                {
                    return CoreHelper.ParseEnum<TE>(value);
                }
                catch (ArgumentException)
                {
                    throw new InputParseFailedException(
                        $"No such {enumName}. Available values are: {string.Join(", ", Enum.GetNames(typeof(TE)))}.");
                }
            });
        }

        public void RegisterInputResolver<T>(Func<AbstractScreen, string, object> supplier)
        {
            resolverMap.Add(typeof(T), supplier);
        }

        [NotNull] public object Parse(AbstractScreen screen, string value, Type type)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException();
            }
            
            var resolver = resolverMap[type];
            var result = resolver.Invoke(screen, value);
            if (result == null)
            {
                throw new Exception();
            }

            return result;
        }
    }
}