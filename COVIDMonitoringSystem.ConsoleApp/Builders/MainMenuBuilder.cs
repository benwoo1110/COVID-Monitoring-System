//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Builders
{
    public class MainMenuBuilder : MenuBuilder
    {
        public MainMenuBuilder(ConsoleDisplayManager displayManager) : base(displayManager)
        {
        }

        protected override void AddSpecialOption()
        {
            TargetScreen.AddElement(new Button("exit")
            {
                Text = "[0] Exit",
                Runner = () => DisplayManager.Stop(),
                BoundingBox = {Top = OptionCount + 5}
            });
        }
    }
}