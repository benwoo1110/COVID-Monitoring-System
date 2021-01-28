using COVIDMonitoringSystem.ConsoleApp.Display;

namespace COVIDMonitoringSystem.ConsoleApp.Screens
{
    public class BuilderAbstractScreen : AbstractScreen
    {
        public string ScreenName { get; set; }
        public override string Name => ScreenName;

        public BuilderAbstractScreen(ConsoleDisplayManager displayManager) : base(displayManager)
        {
        }
    }
}