using System.ComponentModel;
using System.Runtime.CompilerServices;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public abstract class Element
    {
        private bool _hidden = false;
        private string _name;
        public Screen TargetScreen { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        
        public bool Hidden
        {
            get => _hidden;
            set
            {
                _hidden = value;
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

        public virtual void UpdateBox()
        {
            BoundingBox.Top = CHelper.LinesPrinted;
        }

        public abstract void Display();
        
        protected void OnPropertyChanged()
        {
            TargetScreen?.AddToUpdateQueue(this);
        }
    }
}