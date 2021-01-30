//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.TravelEntryMgr
{
    public class GenerateSHNReportScreen : CovidScreen
    {
        public override string Name => "shnReport";
        
        private Input reportDate = new Input("reportDate")
        {
            Prompt = "Report Date",
            BoundingBox = {Top = 4}
        };
        
        public GenerateSHNReportScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }
    }
}