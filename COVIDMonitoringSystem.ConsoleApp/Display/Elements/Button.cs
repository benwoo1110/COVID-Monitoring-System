using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public class Button : SelectableElement
    {
        public Action Runner { get; set; }
        public ButtonMethod MethodRunner { get; set; }

        public Button(string name) : base(name)
        {
        }

        public Button(string name, string text, Action runner) : base(name)
        {
            Text = text;
            Runner = runner;
        }

        public void Run()
        {
            if (MethodRunner != null)
            {
                MethodRunner.Run(TargetAbstractScreen);
                return;
            }
            
            Runner?.Invoke();
        }
    }
}