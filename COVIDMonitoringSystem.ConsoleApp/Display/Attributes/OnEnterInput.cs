using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class OnEnterInput : Attribute
    {
        public string InputName { get; }

        public OnEnterInput(string inputName)
        {
            InputName = inputName;
        }
    }
}