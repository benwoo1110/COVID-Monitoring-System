using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public abstract class SelectableElement : TextElement
    {
        private bool _selected = false;
        private bool _enabled = true;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                OnPropertyChanged();
            }
        }

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged();
            }
        }

        protected SelectableElement(string name) : base(name)
        {
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
            CHelper.WriteLine(Text, Align);
            Console.SetCursorPosition(BoundingBox.Left, BoundingBox.Top);
        }
    }
}