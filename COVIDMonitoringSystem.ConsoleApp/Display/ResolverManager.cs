using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public class ResolverManager
    {
        private Dictionary<Type, Func<AbstractScreen, string, object>> resolverMap;

        public static Func<AbstractScreen, string, object> QuickCreateResolver()
        {
            return (screen, value) =>
            {
                return null;
            };
        }
        
        public ResolverManager()
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
            
            var resolver = resolverMap.GetValueOrDefault(type);
            var result = resolver?.Invoke(screen, value);
            if (result == null)
            {
                throw new Exception();
            }

            return result;
        }
    }
}