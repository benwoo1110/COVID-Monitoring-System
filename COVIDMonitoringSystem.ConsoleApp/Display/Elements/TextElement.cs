using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public abstract class TextElement : Element
    {
        public string Text { get; set; } = "";

        protected TextElement(string name) : base(name)
        {
        }

        protected TextElement(string name, string text) : base(name)
        {
            Text = text;
        }

        public override void Display()
        {
            CHelper.WriteLine(Text);
        }

        public virtual void Clear()
        {
            Text = "";
        }
    }
}