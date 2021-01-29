using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public abstract class Element
    {
        private bool hidden;
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
                OnHiding(value);
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
            if (Hidden)
            {
                return;
            }
            
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

        protected virtual void OnHiding(bool doHiding)
        {
            if (doHiding)
            {
                CHelper.ClearLines(BoundingBox.GetTop(), BoundingBox.GetBottom());
            }
        }

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