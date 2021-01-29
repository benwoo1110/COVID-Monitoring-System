using System;
using System.Collections.Generic;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public class ResolverManager
    {
        private Dictionary<Type, Func<AbstractScreen, string, object>> resolverMap;
        
        public ResolverManager()
        {
            resolverMap = new Dictionary<Type, Func<AbstractScreen, string, object>>();
            
            RegisterInputResolver<string>((screen, value) => value);
        }

        public void RegisterInputResolver<T>(Func<AbstractScreen, string, object> supplier)
        {
            resolverMap.Add(typeof(T), supplier);
        }

        public object Parse(AbstractScreen screen, string value, Type type)
        {
            var resolver = resolverMap.GetValueOrDefault(type);
            return resolver?.Invoke(screen, value);
        }
    }
}