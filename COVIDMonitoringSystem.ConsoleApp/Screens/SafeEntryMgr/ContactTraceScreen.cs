using System;
using System.Linq;
using System.Collections.Generic;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core.SafeEntryMgr;
using COVIDMonitoringSystem.ConsoleApp.Utilities;


namespace COVIDMonitoringSystem.ConsoleApp.Screens.SafeEntryMgr
{
    public class ContactTraceScreen : CovidScreen
    {
        public override string Name => "contactTrace";
        
        public ContactTraceScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }

        private Header header = new Header("header")
        {
            Text = "Generate Contact Tracing Report",
            BoundingBox = { Top = 0 }
        };

        private Input date1 = new Input("date1")
        {
            Prompt = "Enter the beginning of the time period you want to check (dd/mm/yyyy hh:mm)",
            BoundingBox = { Top = 4 }
        };

        private Input date2 = new Input("date2")
        {
            Prompt = "Enter the end of the time period you want to check (dd/mm/yyyy hh:mm)",
            BoundingBox = { Top = 5 }
        };
    }
}