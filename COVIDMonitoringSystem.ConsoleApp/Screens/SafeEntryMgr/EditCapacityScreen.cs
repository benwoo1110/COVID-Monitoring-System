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
using COVIDMonitoringSystem.Core.SafeEntryMgr;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.SafeEntryMgr
{
    public class EditCapacityScreen : CovidScreen
    {
        public override string Name => "changeCapacity";

        private Header header = new Header("header")
        {
            Text = "Change capacity of business location",
            BoundingBox = {Top = 0}
        };

        private Input businessName = new Input("name")
        {
            Prompt = "Enter business name to search for",
            BoundingBox = {Top = 4}
        };

        private Input capacity = new Input("capacity")
        {
            Prompt = "Enter new maximum capacity",
            BoundingBox = {Top = 5}
        };

        private Label divider = new Label("divider")
        {
            Text = "----",
            BoundingBox = {Top = 6}
        };

        private Button confirm = new Button("confirm")
        {
            Text = "[Confirm]",
            BoundingBox = {Top = 7}
        };

        private Label result = new Label("result")
        {
            BoundingBox = {Top = 9},
            ClearOnExit = true
        };

        public EditCapacityScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }

        [OnClick("confirm")]
        private void OnChangeCapacity(
            [InputParam("businessName")] BusinessLocation targetBusiness,
            [InputParam("businessName")] int capacityNumber)
        {
            var oldCapacity = targetBusiness.MaximumCapacity;
            targetBusiness.MaximumCapacity = capacityNumber;
            result.Text = $"Maximum capacity for {targetBusiness} has been changed from {oldCapacity} to {targetBusiness.MaximumCapacity}";
            ClearAllInputs();
        }
    }
}