using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class OnClick : Attribute
    {
        public string ButtonName { get; }
        
        public OnClick(string buttonName)
        {
            ButtonName = buttonName;
        }
    }
}