//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core.SafeEntryMgr;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.SafeEntryMgr
{
    public class ContactTraceScreen : CovidScreen
    {
        public override string Name => "contactTrace";

        private Header header = new Header
        {
            Text = "Generate Contact Tracing Report",
            BoundingBox = {Top = 0}
        };

        private Label locations = new Label
        {
            BoundingBox = {Top = 6}
        };

        private Input date1 = new Input
        {
            Prompt = "Enter the beginning of the time period you want to check (dd/mm/yyyy hh:mm)",
            BoundingBox = {Top = 0}
        };

        private Input date2 = new Input
        {
            Prompt = "Enter the end of the time period you want to check (dd/mm/yyyy hh:mm)",
            BoundingBox = {Top = 1}
        };

        private Input targetStore = new Input
        {
            Prompt = "Enter the name of the store you want to check",
            BoundingBox = {Top = 2},
            SuggestionType = "businessLocation"
        };

        private Button generate = new Button
        {
            Text = "[Generate report]",
            BoundingBox = {Top = 3}
        };

        private Label nameList = new Label
        {
            BoundingBox = {Top = 5}
        };

        private Label output = new Label
        {
            BoundingBox = {Top = 9},
            ClearOnExit = true
        };

        public ContactTraceScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(
            displayManager, covidManager)
        {
            date1.BoundingBox.SetRelativeElement(locations);
            date2.BoundingBox.SetRelativeElement(locations);
            targetStore.BoundingBox.SetRelativeElement(locations);
            generate.BoundingBox.SetRelativeElement(locations);
            nameList.BoundingBox.SetRelativeElement(locations);
            output.BoundingBox.SetRelativeElement(locations);
        }

        public override void PreLoad()
        {
            ShowLocations();
        }

        private void ShowLocations()
        {
            var locationNames = "Available Business Locations:\n";


            foreach (var i in CovidManager.BusinessLocationList)
            {
                locationNames += $"{i.BusinessName}\n";
            }

            locations.Text = locationNames;
        }

        [OnClick("generate")]
        private void OnContactTracing(
            [InputParam("date1", "output")] DateTime targetDate1,
            [InputParam("date2", "output")] DateTime targetDate2,
            [InputParam("targetStore", "output")] BusinessLocation targetLocation)
        {
            var contactNames = "";

            foreach (var i in CovidManager.PersonList)
            {
                foreach (var n in i.SafeEntryList)
                {
                    if (n.Location == targetLocation && n.CheckIn >= targetDate1 && n.CheckIn <= targetDate2)
                    {
                        contactNames += i.Name + "\n";
                    }
                }
            }

            nameList.Text = contactNames;

            output.Text = CovidManager.GenerateContactTracingReportFile(targetLocation, targetDate1, targetDate2)
                ? "Successfully generated report file."
                : "There was an error generating report file.";
        }
    }
}