﻿//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
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

        private Header header = new Header("header")
        {
            Text = "SafeEntry Check In",
            BoundingBox = { Top = 0 }
        };

        private Label locations = new Label("locations")
        {
            BoundingBox = { Top = 4 }
        };

        private Input name = new Input("name")
        {
            Prompt = "Enter your name",
            BoundingBox = { Top = 0 }
        };

        private Input targetStore = new Input("targetStore")
        {
            Prompt = "Enter business location to check in to",
            BoundingBox = { Top = 1 }
        };

        private Label divider = new Label("divider")
        {
            Text = "----",
            BoundingBox = { Top = 2 }
        };

        private Button confirm = new Button("confirm")
        {
            Text = "[Check In]",
            BoundingBox = { Top = 3 }
        };

        private Label result = new Label("result")
        {
            BoundingBox = { Top = 6 },
            ClearOnExit = true
        };

        public CheckInScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(
            displayManager, covidManager)
        {
            name.BoundingBox.SetRelativeBox(locations);
            targetStore.BoundingBox.SetRelativeBox(locations);
            divider.BoundingBox.SetRelativeBox(locations);
            confirm.BoundingBox.SetRelativeBox(locations);
            result.BoundingBox.SetRelativeBox(locations);
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

        [OnClick("confirm")] private void OnCheckIn(
            [Parser("name", "result")] Resident person,
            [Parser("targetStore", "result")] BusinessLocation location)
        {
            var inputName = CovidManager.FindPerson(name.Text);
            var inputLocation = CovidManager.FindBusinessLocation(targetStore.Text);
            if (inputName != null && inputLocation != null)
            {
                if (inputLocation.IsFull())
                {
                    result.Text = $"{inputLocation} is at full capacity, please try again later.";
                }
                else
                {
                    var checkIn = new SafeEntry(DateTime.Now, inputLocation);
                    inputName.AddSafeEntry(checkIn);
                    inputLocation.VisitorsNow += 1;
                    result.Text = $"You have been checked in to {inputLocation}";
                }

            }
            name.ClearText();
            targetStore.ClearText();
            ClearAllInputs();
        }
    }
}
