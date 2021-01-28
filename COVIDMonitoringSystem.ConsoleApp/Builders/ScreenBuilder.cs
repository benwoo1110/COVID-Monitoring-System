using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Screens;

namespace COVIDMonitoringSystem.ConsoleApp.Builders
{
    public class ScreenBuilder : AbstractScreenBuilder<ScreenBuilder, BuilderScreen>
    {
        //TODO REMOVE THIS
        public ScreenBuilder(ConsoleDisplayManager displayManager) : base(displayManager)
        {
        }
    }
}