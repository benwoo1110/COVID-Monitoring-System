using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class Values : Attribute
    {
        public string ValueName { get; }

        public Values(string valueName)
        {
            ValueName = valueName;
        }
    }
}