using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.Core;

namespace COVIDMonitoringSystem.ConsoleApp.Screens
{
    public abstract class CovidScreen : AbstractScreen
    {
        protected COVIDMonitoringManager CovidManager { get; set; }

        protected CovidScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager)
        {
            CovidManager = covidManager;
        }
    }
}