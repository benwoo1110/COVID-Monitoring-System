using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public abstract class Element
    {
        private bool hidden = false;
        private string name;
        public Screen TargetScreen { get; set; }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        
        public bool Hidden
        {
            get => hidden;
            set
            {
                hidden = value;
                OnPropertyChanged();
            }
        }
        
        public Box BoundingBox { get; }

        protected Element()
        {
            BoundingBox = new Box();
        }

        protected Element(string name)
        {
            Name = name;
            BoundingBox = new Box();
        }

        public virtual void Render()
        {
            BoundingBox.SetDrawPosition();
            SelectColour();
            WriteToScreen();
            BoundingBox.SetCursorPosition();
        }

        protected virtual void SelectColour()
        {
            ColourSelector.Element();
        }

        protected abstract void WriteToScreen();

        protected void OnPropertyChanged()
        {
            TargetScreen?.AddToUpdateQueue(this);
        }
    }
}