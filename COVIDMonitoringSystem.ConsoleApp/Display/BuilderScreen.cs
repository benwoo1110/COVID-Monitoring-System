//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public class BuilderScreen : AbstractScreen
    {
        public string ScreenName { get; set; }
        public override string Name => ScreenName;

        public BuilderScreen(ConsoleDisplayManager displayManager) : base(displayManager)
        {
        }
    }
}