using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using COVIDMonitoringSystem.ConsoleApp.Builders;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.SafeEntryMgr;


namespace COVIDMonitoringSystem.ConsoleApp.Screens
{
    public class ViewLocationsScreen : CovidScreen
    {
        public override string Name => "viewLocations";

        private Header header = new Header("header")
        {
            Text = "View business locations",
            BoundingBox = { Top = 0 }
        };

        // ScreenBuilder?

        public ViewLocationsScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {

        }
    }
}
