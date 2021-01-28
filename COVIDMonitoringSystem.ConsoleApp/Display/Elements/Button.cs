using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public class Button : SelectableElement
    {
        public Action Runner { get; set; }

        public Button(string name) : base(name)
        {
        }

        public Button(string name, string text, Action runner) : base(name)
        {
            Text = text;
            Runner = runner;
        }
    }
}