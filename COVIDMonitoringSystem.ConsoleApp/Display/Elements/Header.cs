//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

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

        protected override int WriteToScreen()
        {
            return CHelper.WriteLine($"\n{Text}\n", Align);
        }

        protected override void SelectColour()
        {
            ColourSelector.Header();
        }
    }
}