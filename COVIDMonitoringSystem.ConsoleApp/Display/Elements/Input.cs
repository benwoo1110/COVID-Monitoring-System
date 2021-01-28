using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public class Input : SelectableElement
    {
        private string prompt;

        private string Prompt
        {
            get => prompt;
            set
            {
                prompt = value;
                UpdateCursor();
                OnPropertyChanged();
            }
        }

        public override string Text
        {
            get => text;
            set
            {
                text = value;
                UpdateCursor();
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

        public void UpdateCursor()
        {
            //TODO: Improve this
            BoundingBox.CursorLeft = Prompt.Length + Text.Length + 6;
        }
    }
}