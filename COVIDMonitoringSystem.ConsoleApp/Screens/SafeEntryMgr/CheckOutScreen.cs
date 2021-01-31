//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Linq;
using System.Collections.Generic;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.SafeEntryMgr;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.SafeEntryMgr
{
    public class CheckOutScreen : CovidScreen
    {
        public override string Name => "checkOut";

        private Header header = new Header
        {
            Text = "SafeEntry Check Out",
            BoundingBox = { Top = 0 }
        };
        private Input name = new Input
        {
            Prompt = "Enter your name",
            BoundingBox = { Top = 4 },
            SuggestionType = "person"
        };
        private Button check = new Button
        {
            Text = "[Check]",
            BoundingBox = { Top = 5 }
        };
        private Label locations = new Label
        {
            ClearOnExit = true,
            BoundingBox = { Top = 6 }
        };
        private Label divider = new Label
        {
            Text = "----",
            BoundingBox = { Top = 1 }
        };
        private Input targetStore = new Input
        {
            Prompt = "Enter business location to check out from",
            Hidden = true,
            Enabled = false,
            BoundingBox = { Top = 2 },
            SuggestionType = "businessLocation"
        };
        private Button confirm = new Button
        {
            Text = "[Check Out]",
            Hidden = true,
            Enabled = false,
            BoundingBox = { Top = 3 }
        };
        private Label result = new Label
        {
            ClearOnExit = true,
            BoundingBox = { Top = 4 }
        };

        public CheckOutScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(
            displayManager, covidManager)
        {
            targetStore.BoundingBox.SetRelativeElement(locations);
            divider.BoundingBox.SetRelativeElement(locations);
            confirm.BoundingBox.SetRelativeElement(locations);
            result.BoundingBox.SetRelativeElement(locations);
        }

        public override void PreLoad()
        {
            targetStore.Hidden = true;
            targetStore.Enabled = false;
            confirm.Hidden = true;
            confirm.Enabled = false;
        }

        [OnClick("check")]
        private void OnShowLocations(
            [InputParam("name", "locations")] Person targetPerson)
        {
            var locationNames = "Available Business Locations:\n";
            var latestCheckinDate = new List<DateTime>();
            var latestCheckoutDate = new List<DateTime>();

            foreach (var i in targetPerson.SafeEntryList)
            {
                latestCheckinDate.Add(i.CheckIn);
                latestCheckoutDate.Add(i.CheckOut);
                if (latestCheckinDate.Max() > latestCheckoutDate.Max())
                {
                    locationNames += $"{i.Location}\n";
                }
            }

            locations.Text = locationNames;
            targetStore.Hidden = false;
            targetStore.Enabled = true;
            confirm.Hidden = false;
            confirm.Enabled = true;
        }

        [OnClick("confirm")] 
        private void OnCheckOut(
            [InputParam("targetStore", "result")] BusinessLocation location)
        {
            var inputName = CovidManager.FindPerson(name.Text);
            location = CovidManager.FindBusinessLocation(targetStore.Text);
            var latestCheckinDate = new List<DateTime>();
            var latestCheckoutDate = new List<DateTime>();
            foreach (var i in inputName.SafeEntryList)
            {
                latestCheckinDate.Add(i.CheckIn);
                latestCheckoutDate.Add(i.CheckOut);
                if (i.Location == location && latestCheckinDate.Max() > latestCheckoutDate.Max())
                {
                    i.PerformCheckOut();
                    location.VisitorsNow -= 1;
                    result.Text = $"You have been checked out from {location}";
                    ClearAllInputs();
                    return;
                }
            }
        }
    }
}