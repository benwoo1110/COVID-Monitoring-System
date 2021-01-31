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

            public ValueTypeMatcher(string name)
            {
                Name = name;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Name);
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

        public void RegisterInputValueType(string name, Func<AbstractScreen, ICollection<string>> supplier, string errorMessage = null)
        {
            valuesMap.Add(new ValueTypeMatcher(name).GetHashCode(), new ValueChecker(supplier, errorMessage));
        }
        
        public void DoCheck(string name, AbstractScreen screen, object obj)
        {
            var matcher = new ValueTypeMatcher(name);
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

        public List<string> GetSuggestion(string name, AbstractScreen screen, object obj)
        {
            var result = new List<string>();
            var matcher = new ValueTypeMatcher(name);
            var valueChecker = valuesMap.GetValueOrDefault(matcher.GetHashCode());
            if (valueChecker == null)
            {
                return result;
            }

            result.AddRange(valueChecker.ListGetter(screen)
                .Where(s => s.ToLower().StartsWith(obj.ToString()?.ToLower() ?? "")));

            return result;
        }
    }
}