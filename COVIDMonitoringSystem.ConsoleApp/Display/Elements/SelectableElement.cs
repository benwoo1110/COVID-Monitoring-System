namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public abstract class SelectableElement : TextElement
    {
        public bool Enabled { get; set; } = true;

        protected SelectableElement(string name) : base(name)
        {
        }
    }
}