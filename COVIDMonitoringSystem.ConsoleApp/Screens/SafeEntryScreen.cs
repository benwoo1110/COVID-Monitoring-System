using System;
using System.Collections.Generic;
using System.Text;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core;


namespace COVIDMonitoringSystem.ConsoleApp.Screens
{
    public class SafeEntryScreen : CovidScreen
    {
        public override string Name => "safeEntry";

        private Header header = new Header("header")
        {
            Text = "SafeEntry Manager",
            BoundingBox = { Top = 0 }
        };

        private Button assignToken = new Button("assignToken")
        {
            Text = "[Assign or replace TraceTogether Token]",
            BoundingBox = { Top = 4 }
        };

        private Button viewLocations = new Button("viewLocations")
        {
            Text = "[View all business locations]",
            BoundingBox = { Top = 5 }
        };

        private Button editCapacity = new Button("editCapacity")
        {
            Text = "[Change capacity of business location]",
            BoundingBox = { Top = 6 }
        };

        private Button checkIn = new Button("checkIn")
        {
            Text = "[SafeEntry Check-In]",
            BoundingBox = { Top = 7 }
        };

        private Button checkOut = new Button("checkOut")
        {
            Text = "[SafeEntry Check-Out]",
            BoundingBox = { Top = 8 }
        };

        private Label separator = new Label("separator")
        {
            Text = "----",
            BoundingBox = { Top = 9 }
        };

        private Button back = new Button("back")
        {
            Text = "[Back]",
            BoundingBox = { Top = 10 }
        };

        public SafeEntryScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }
    }
}
