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
            BoundingBox = { Top = 8 }
        };

        private Input targetStore = new Input("targetStore")
        {
            Prompt = "Enter business location to check in to",
            BoundingBox = { Top = 9 }
        };

        private Label divider = new Label("divider")
        {
            Text = "----",
            BoundingBox = { Top = 10 }
        };

        private Button confirm = new Button("confirm")
        {
            Text = "[Check In]",
            BoundingBox = { Top = 11 }
        };

        private Label result = new Label("result")
        {
            BoundingBox = { Top = 13 }
        };

        
        /*private void ShowLocations()
        {
            List<string> locationNames = new List<string>();
            foreach (var i in CovidManager.BusinessLocationList)
            {
                locationNames.Add(i.BusinessName);
            }
            foreach (var i in locationNames)
            {
                locations.Text =
            }
            
        }*/
        private void CheckIn()
        {
            var inputName = CovidManager.FindPerson(name.Text);
            var inputLocation = CovidManager.FindBusinessLocation(locations.Text);
            Console.WriteLine(inputName);
            Console.WriteLine(inputLocation);
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
            else
            {
                result.Text = $"Either name or location or both does not exist. You are not checked in.";
            }
            

        }
        [OnClick("confirm")]private void OnConfirm()
        {
            CheckIn();
            name.ClearText();
            targetStore.ClearText();
        }
        public CheckInScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
            // ShowLocations();
            // How to display all stores using .Text and Label?
        }
    }
}
