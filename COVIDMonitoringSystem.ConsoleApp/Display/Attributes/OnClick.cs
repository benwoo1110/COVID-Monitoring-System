using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class OnClick : Attribute
    {
        public string ButtonName { get; }
        
        public OnClick(string buttonName)
        {
            ButtonName = buttonName;
        }
    }
}