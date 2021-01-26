using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public class Header : TextElement
    {
        public override TextAlign Align { get; set; } = TextAlign.Center;
        
        public Header(string name) : base(name)
        {
        }

        public Header(string name, string text) : base(name, text)
        {
        }

        public override void Display()
        {
            ColourSelector.Header();
            CHelper.WriteEmpty();
            CHelper.WriteLine(Text, Align);
            CHelper.WriteEmpty();
        }
    }
}