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

        protected override void WriteToScreen()
        {
            CHelper.WriteEmpty();
            CHelper.WriteLine(Text, Align);
            CHelper.WriteEmpty();
        }

        protected override void SelectColour()
        {
            ColourSelector.Header();
        }
    }
}