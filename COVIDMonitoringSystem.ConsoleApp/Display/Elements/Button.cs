using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public class Button : SelectableElement
    {
        public Action Run { get; set; }
    }
}