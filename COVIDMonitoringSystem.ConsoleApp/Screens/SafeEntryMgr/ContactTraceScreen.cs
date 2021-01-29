﻿using System;
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
            date1.BoundingBox.SetRelativeBox(locations);
            date2.BoundingBox.SetRelativeBox(locations);
            targetStore.BoundingBox.SetRelativeBox(locations);
            generate.BoundingBox.SetRelativeBox(locations);
            nameList.BoundingBox.SetRelativeBox(locations);
            output.BoundingBox.SetRelativeBox(locations);
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

        [OnClick("generate")]private void onGenerate()
        {
            ContactTracing();
        }

        private void ContactTracing()
        {
            var targetDate1 = Convert.ToDateTime(date1.Text);
            var targetDate2 = Convert.ToDateTime(date2.Text);
            var targetLocation = CovidManager.FindBusinessLocation(targetStore.Text);
            var contactNames = "";

            if (targetLocation != null)
            {
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
                var result = CovidManager.GenerateContactTracingReportFile(targetLocation, targetDate1, targetDate2)
                    ? output.Text = "Successfully generated report file."
                    : output.Text = "There was an error generating report file.";

            }
        }
        private Header header = new Header("header")
        {
            Text = "Generate Contact Tracing Report",
            BoundingBox = { Top = 0 }
        };

        private Label locations = new Label("locations")
        {
            BoundingBox = { Top = 6 }
        };

        private Input date1 = new Input("date1")
        {
            Prompt = "Enter the beginning of the time period you want to check (dd/mm/yyyy hh:mm)",
            BoundingBox = { Top = 0 }
        };

        private Input date2 = new Input("date2")
        {
            Prompt = "Enter the end of the time period you want to check (dd/mm/yyyy hh:mm)",
            BoundingBox = { Top = 1 }
        };

        private Input targetStore = new Input("targetStore")
        {
            Prompt = "Enter the name of the store you want to check",
            BoundingBox = { Top = 2 }
        };

        private Button generate = new Button("generate")
        {
            Text = "[Generate report]",
            BoundingBox = { Top = 3 }
        };

        private Label nameList = new Label("nameList")
        {
            BoundingBox = { Top = 5 }
        };

        private Label output = new Label("output")
        {
            BoundingBox = { Top = 9 }
        };
    }
}