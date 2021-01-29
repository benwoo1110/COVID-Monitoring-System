using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public abstract class Element
    {
        private bool hidden = false;
        private string name;
        public AbstractScreen TargetAbstractScreen { get; set; }

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

        //TODO: Have screen input here so can register
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
            BoundingBox.UpdateHeight(WriteToScreen());
            BoundingBox.SetCursorPosition();
        }

        protected virtual void SelectColour()
        {
            ColourSelector.Element();
        }

        protected abstract int WriteToScreen();

        protected void OnPropertyChanged()
        {
            if (TargetAbstractScreen == null || !TargetAbstractScreen.Active)
            {
                return;
            }
            
            TargetAbstractScreen.AddToUpdateQueue(this);
        }
    }
}