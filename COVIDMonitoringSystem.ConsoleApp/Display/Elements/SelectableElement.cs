//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public abstract class SelectableElement : TextElement
    {
        private bool selected;
        private bool enabled = true;

        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                QueueToRerender();
            }
        }

        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                if (value == false)
                {
                    OnDeselect();
                }
                QueueToRerender();
            }
        }

        protected SelectableElement(string name = null) : base(name)
        {
        }

        public bool IsSelectable()
        {
            return Enabled && !Hidden;
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

        protected virtual void OnDeselect()
        {
        }
    }
}