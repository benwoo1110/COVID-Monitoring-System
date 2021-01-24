using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public class Input : SelectableElement
    {
        public string Prompt { get; set; }

        public Input()
        {
        }

        public Input(string prompt)
        {
            Prompt = prompt;
        }

        public override void UpdateBox()
        {
            base.UpdateBox();
            UpdateWidth();
        }

        public override void Display()
        {
            CHelper.WriteLine($"{Prompt}: {Text}");
        }

        public void UpdateWidth()
        {
            BoundingBox.Left = Prompt.Length + Text.Length + 6;
        }
    }
}