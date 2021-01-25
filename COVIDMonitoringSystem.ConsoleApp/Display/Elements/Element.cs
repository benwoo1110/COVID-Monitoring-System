using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public abstract class Element
    {
        public Screen TargetScreen { get; set; }
        public string Name { get; set; }
        public Box BoundingBox { get; set; }
        public bool Hidden { get; set; } = false;

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
    }
}