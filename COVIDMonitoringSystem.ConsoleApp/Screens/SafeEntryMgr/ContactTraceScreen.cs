using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.Core;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.SafeEntryMgr
{
    public class ContactTraceScreen : CovidScreen
    {
        public override string Name => "contactTrace";
        
        public ContactTraceScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }
    }
}