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

        public override void Display()
        {
            if (!Enabled)
            {
                //TODO: Disabled color
            }
            else if (Selected)
            {
                ColourSelector.Selected();
            }
            else
            {
                ColourSelector.Element();
            }
            
            Console.SetCursorPosition(0, BoundingBox.Top);
            CHelper.WriteLine($"{Prompt}: {Text}", Align);
            Console.SetCursorPosition(BoundingBox.Left, BoundingBox.Top);
        }

        public void UpdateWidth()
        {
            BoundingBox.Left = Prompt.Length + Text.Length + 6;
        }
    }
}