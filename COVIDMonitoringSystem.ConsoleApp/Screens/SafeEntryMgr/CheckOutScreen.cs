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
    public class CheckOutScreen : CovidScreen
    {
        public override string Name => "checkOut";

        private Header header = new Header("header")
        {
            Text = "SafeEntry Check Out",
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
            Prompt = "Enter business location to check out from",
            BoundingBox = { Top = 9 }
        };

        private Label divider = new Label("divider")
        {
            Text = "----",
            BoundingBox = { Top = 10 }
        };

        private Button confirm = new Button("confirm")
        {
            Text = "[Check Out]",
            BoundingBox = { Top = 11 }
        };

        private Label result = new Label("result")
        {
            BoundingBox = { Top = 13 }
        };


        private void CheckOut()
        {
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
                        CHelper.WriteLine(i.ToString());
                    }
                }
                var checkoutLocation = CHelper.GetInput("Enter store to check out from: ", CovidManager.FindBusinessLocation);
                foreach (var i in inputName.SafeEntryList)
                {
                    if (i.Location == checkoutLocation && latestCheckinDate.Max() > latestCheckoutDate.Max())
                    {
                        i.PerformCheckOut();
                        checkoutLocation.VisitorsNow -= 1;
                        CHelper.WriteLine("You have been checked out from " + checkoutLocation);
                        return;
                    }
                }

                CHelper.WriteLine("You are not checked in to this store");
            }
            else
            {
                CHelper.WriteLine("Name not found.");
            }
        }
        public CheckOutScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }
    }
}