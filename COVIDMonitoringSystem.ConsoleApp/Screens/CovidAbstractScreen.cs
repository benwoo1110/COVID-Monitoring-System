using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.Core;

namespace COVIDMonitoringSystem.ConsoleApp.Screens
{
    public abstract class CovidAbstractScreen : AbstractScreen
    {
        protected COVIDMonitoringManager CovidManager { get; set; }

        protected CovidAbstractScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager)
        {
            CovidManager = covidManager;
        }
    }
}