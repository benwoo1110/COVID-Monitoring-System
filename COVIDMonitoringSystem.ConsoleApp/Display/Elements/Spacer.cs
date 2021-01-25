using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public class Spacer : Element
    {
        public Spacer()
        {
            Name = "spacer";
        }

        public override void Display()
        {
            CHelper.WriteEmpty();
        }
    }
}