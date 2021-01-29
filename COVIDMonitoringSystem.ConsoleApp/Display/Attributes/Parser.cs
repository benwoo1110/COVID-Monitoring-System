using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class Parser : Attribute
    {
        public string InputName { get; }
        public string TargetLabel { get; }

        public Parser(string inputName, string targetLabel)
        {
            InputName = inputName;
            TargetLabel = targetLabel;
        }
    }
}