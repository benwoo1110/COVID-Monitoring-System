using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public class Input : SelectableElement
    {
        public string Prompt { get; set; }

        public override string Text
        {
            get => _text;
            set
            {
                _text = value;
                UpdateWidth();
                OnPropertyChanged();
            }
        }

        public Input(string name, string prompt) : base(name)
        {
            Prompt = prompt;
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