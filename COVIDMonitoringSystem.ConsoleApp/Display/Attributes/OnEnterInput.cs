using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class OnEnterInput: Attribute
    {
        public string InputName { get; }

        public OnEnterInput(string inputName)
        {
            InputName = inputName;
        }
    }
}