//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Text;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.SafeEntryMgr;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.SafeEntryMgr
{
    public class CheckInScreen : CovidScreen
    {
        public override string Name => "checkIn";

        private Header header = new Header
        {
            Text = "SafeEntry Check In",
            BoundingBox = {Top = 0}
        };

        private Label locations = new Label
        {
            BoundingBox = {Top = 4}
        };

        private Input name = new Input
        {
            Prompt = "Enter your name",
            BoundingBox = {Top = 0},
            SuggestionType = "person"
        };

        private Input targetStore = new Input
        {
            Prompt = "Enter business location to check in to",
            BoundingBox = {Top = 1},
            SuggestionType = "businessLocation"
        };

        private Label divider = new Label
        {
            Text = "----",
            BoundingBox = {Top = 2}
        };

        private Button confirm = new Button
        {
            Text = "[Check In]",
            BoundingBox = {Top = 3}
        };

        private Label result = new Label
        {
            BoundingBox = {Top = 6},
            ClearOnExit = true
        };

        public CheckInScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(
            displayManager, covidManager)
        {
            name.BoundingBox.SetRelativeElement(locations);
            targetStore.BoundingBox.SetRelativeElement(locations);
            divider.BoundingBox.SetRelativeElement(locations);
            confirm.BoundingBox.SetRelativeElement(locations);
            result.BoundingBox.SetRelativeElement(locations);
        }

        public override void PreLoad()
        {
            ShowLocations();
        }

        private void ShowLocations()
        {
            var locationNames = new StringBuilder("Available Business Locations:\n");

            foreach (var i in CovidManager.BusinessLocationList)
            {
                locationNames.Append($"{i.BusinessName}\n");
            }

            locations.Text = locationNames.ToString();
        }

        [OnClick("confirm")]
        private void OnCheckIn(
            [InputParam("name", "result")] Person person,
            [InputParam("targetStore", "result")] BusinessLocation location)
        {
            ClearAllInputs();
            if (location.IsFull())
            {
                result.Text = $"{location} is at full capacity, please try again later.";
                return;
            }

            var checkIn = new SafeEntry(DateTime.Now, location);
            person.AddSafeEntry(checkIn);
            location.VisitorsNow += 1;
            result.Text = $"You have been checked in to {location}";
        }
    }
}