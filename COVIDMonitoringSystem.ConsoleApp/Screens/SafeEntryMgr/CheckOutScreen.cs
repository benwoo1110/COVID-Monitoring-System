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

        private Header header = new Header("header")
        {
            Text = "SafeEntry Check Out",
            BoundingBox = { Top = 0 }
        };

        private Input name = new Input("name")
        {
            Prompt = "Enter your name",
            BoundingBox = { Top = 4 }
        };

        private Button check = new Button("check")
        {
            Text = "[Check]",
            BoundingBox = { Top = 5 }
        };

        private Label locations = new Label("locations")
        {
            ClearOnExit = true,
            BoundingBox = { Top = 6 }
        };

        private Label divider = new Label("divider")
        {
            Text = "----",
            BoundingBox = { Top = 4 }
        };

        private Input targetStore = new Input("targetStore")
        {
            Prompt = "Enter business location to check out from",
            Hidden = true,
            Enabled = false,
            BoundingBox = { Top = 5 }
        };

        private Button confirm = new Button("confirm")
        {
            Text = "[Check Out]",
            Hidden = true,
            Enabled = false,
            BoundingBox = { Top = 6 }
        };

        private Label result = new Label("result")
        {
            ClearOnExit = true,
            BoundingBox = { Top = 8 }
        };

        [OnClick("check")]private void OnShowLocations(
            [InputParam("name", "locations")] Resident person)
        {
            var locationNames = "Available Business Locations:\n";
            var inputName = CovidManager.FindPerson(name.Text);
            List<DateTime> latestCheckinDate = new List<DateTime>();
            List<DateTime> latestCheckoutDate = new List<DateTime>();
            if (inputName != null)
            {
                foreach (var i in inputName.SafeEntryList)
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
            else
            {
                locations.Text = $"{inputName} is not a valid resident.";
            }
        }

        [OnClick("confirm")] private void OnCheckOut(
            [InputParam("targetStore", "result")] BusinessLocation location)
        {
            var inputName = CovidManager.FindPerson(name.Text);
            var checkoutLocation = CovidManager.FindBusinessLocation(targetStore.Text);
            List<DateTime> latestCheckinDate = new List<DateTime>();
            List<DateTime> latestCheckoutDate = new List<DateTime>();
            foreach (var i in inputName.SafeEntryList)
            {
                latestCheckinDate.Add(i.CheckIn);
                latestCheckoutDate.Add(i.CheckOut);
                if (i.Location == checkoutLocation && latestCheckinDate.Max() > latestCheckoutDate.Max())
                {
                    i.PerformCheckOut();
                    checkoutLocation.VisitorsNow -= 1;
                    result.Text = $"You have been checked out from {checkoutLocation}";
                    ClearAllInputs();
                    return;
                }
            }
        }
        public override void PreLoad()
        {
            targetStore.Hidden = true;
            targetStore.Enabled = false;
            confirm.Hidden = true;
            confirm.Enabled = false;
        }

        public CheckOutScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
            targetStore.BoundingBox.SetRelativeElement(locations);
            divider.BoundingBox.SetRelativeElement(locations);
            confirm.BoundingBox.SetRelativeElement(locations);
            result.BoundingBox.SetRelativeElement(locations);
        }
        
    }
}