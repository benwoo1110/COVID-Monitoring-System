//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System.Collections.Generic;
using System.Text;

namespace COVIDMonitoringSystem.ConsoleApp.Utilities
{
    public class DetailsBuilder
    {
        private StringBuilder Builder { get; }

        public DetailsBuilder()
        {
            Builder = new StringBuilder();
        }

        public DetailsBuilder AddInfo<T>(string key, T value)
        {
            Builder.Append($"{key}: {value}\n");
            return this;
        }

        public DetailsBuilder Separator()
        {
            Builder.Append("\n");
            return this;
        }
        
        public DetailsBuilder AddOrderedList<T>(string key, List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                Builder.Append($"{key}: None\n");
                return this;
            }
            
            Builder.Append($"{key}:\n");
            for (var i = 0; i < list.Count; i++)
            {
                Builder.Append($"  {i+1}. {list[i]}\n");
            }
            return this;
        }

        public string Build()
        {
            return Builder.ToString();
        }
    }
}