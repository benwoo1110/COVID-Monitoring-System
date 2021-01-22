using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public class Option
    {
        public string Name { get; }
        public Action RunMethod { get; }

        public Option(string name, Action runMethod)
        {
            Name = name;
            RunMethod = runMethod;
        }
    }
}