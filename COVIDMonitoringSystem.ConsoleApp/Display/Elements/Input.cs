using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public class Input : SelectableElement
    {
        public string Prompt { get; set; }

        public Input(string name, string prompt) : base(name)
        {
            Prompt = prompt;
        }

        public override void UpdateBox()
        {
            base.UpdateBox();
            UpdateWidth();
        }

        protected override void WriteToScreen()
        {
            CHelper.WriteLine($"{Prompt}: {Text}", Align);
        }

        public void UpdateWidth()
        {
            BoundingBox.Left = Prompt.Length + Text.Length + 6;
        }
    }
}