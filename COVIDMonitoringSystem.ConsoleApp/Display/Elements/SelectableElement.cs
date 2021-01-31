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
        protected bool selected;
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

        public virtual bool Selected
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
                ColourSelector.Disabled();
            }
            else if (Selected)
            {
                ColourSelector.Selected();
            }
            else
            {
                ColourSelector.Default();
            }
        }

        protected virtual void OnDeselect()
        {
        }
    }
}