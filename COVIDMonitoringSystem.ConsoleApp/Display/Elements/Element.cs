//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public abstract class Element
    {
        private bool hidden;
        private string name;
        public AbstractScreen TargetScreen { get; set; }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                QueueToRerender();
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
            BoundingBox = new Box(this);
        }

        protected Element(string name)
        {
            Name = name;
            BoundingBox = new Box(this);
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
            if (!doHiding)
            {
                QueueToRerender();
                return;
            }
            
            CHelper.ClearLines(BoundingBox.GetTop(), BoundingBox.GetBottom());
            if (BoundingBox.AutoHeight)
            {
                BoundingBox.Height = 0;
            }
        }

        public void QueueToRerender()
        {
            if (TargetScreen == null || !TargetScreen.Active)
            {
                return;
            }
            
            TargetScreen.AddToUpdateQueue(this);
        }
    }
}