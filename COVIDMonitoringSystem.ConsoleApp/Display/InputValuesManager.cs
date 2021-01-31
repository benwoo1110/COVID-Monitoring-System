using System;
using System.Collections.Generic;
using System.Linq;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public class InputValuesManager
    {
        private class ValueTypeMatcher
        {
            public string Name { get; }
            public Type TargetType { get; }

            public ValueTypeMatcher(string name, Type targetType)
            {
                Name = name;
                TargetType = targetType;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Name, TargetType);
            }
        }

        private class ValueChecker
        {
            public Func<AbstractScreen, ICollection<string>> ListGetter { get; }
            public string ErrorMessage { get; }

            public ValueChecker(Func<AbstractScreen, ICollection<string>> listGetter, string errorMessage)
            {
                ListGetter = listGetter;
                ErrorMessage = errorMessage;
            }
        }
        
        private Dictionary<int, ValueChecker> valuesMap;

        public InputValuesManager()
        {
            valuesMap = new Dictionary<int, ValueChecker>();
        }

        public void RegisterInputValueType<T>(string name, Func<AbstractScreen, ICollection<string>> supplier, string errorMessage)
        {
            valuesMap.Add(new ValueTypeMatcher(name, typeof(T)).GetHashCode(), new ValueChecker(supplier, errorMessage));
        }
        
        public void DoCheck(string name, Type type, AbstractScreen screen, object obj)
        {
            var matcher = new ValueTypeMatcher(name, type);
            var valueChecker = valuesMap.GetValueOrDefault(matcher.GetHashCode());
            if (valueChecker == null)
            {
                return;
            }
            
            var resultList = valueChecker.ListGetter.Invoke(screen);
            if (resultList.FirstOrDefault(s => s.ToLower().Equals(obj.ToString()?.ToLower())) == null)
            {
                throw new InputParseFailedException(string.Format(valueChecker.ErrorMessage, obj));
            }
        }
    }
}