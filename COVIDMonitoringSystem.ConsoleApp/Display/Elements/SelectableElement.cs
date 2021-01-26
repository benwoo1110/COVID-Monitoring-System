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

        protected override void SelectColour()
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
        }
    }
}