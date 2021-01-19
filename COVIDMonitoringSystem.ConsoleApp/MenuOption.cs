using System;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public class MenuOption
    {
        public string Name { get; }
        public Action RunMethod { get; }

        public MenuOption(string name, Action runMethod)
        {
            Name = name;
            RunMethod = runMethod;
        }
    }
}