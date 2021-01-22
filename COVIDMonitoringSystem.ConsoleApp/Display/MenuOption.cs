using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public class MenuOption : Option
    {
        public MenuOption(string name, Action runMethod) : base(name, runMethod)
        {
        }
    }
}