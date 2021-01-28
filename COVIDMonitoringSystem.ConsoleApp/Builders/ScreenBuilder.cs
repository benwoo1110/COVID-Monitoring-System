using COVIDMonitoringSystem.ConsoleApp.Display;

namespace COVIDMonitoringSystem.ConsoleApp.Builders
{
    public class ScreenBuilder : AbstractScreenBuilder<ScreenBuilder, Screen>
    {
        public ScreenBuilder(ConsoleDisplayManager displayManager) : base(displayManager)
        {
        }
    }
}