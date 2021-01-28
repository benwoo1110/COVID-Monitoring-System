using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.Core;

namespace COVIDMonitoringSystem.ConsoleApp.Screens
{
    public abstract class CovidScreen : Screen
    {
        protected COVIDMonitoringManager CovidManager { get; set; }

        protected CovidScreen(ConsoleDisplayManager displayDisplayManager, COVIDMonitoringManager covidManager) : base(displayDisplayManager)
        {
            CovidManager = covidManager;
        }
    }
}