//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.SafeEntryMgr
{
    public class EditCapacityScreen : CovidScreen
    {
        public override string Name => "changeCapacity";

        private Header header = new Header("header")
        {
            Text = "Change capacity of business location",
            BoundingBox = { Top = 0 }
        };

        private Input businessName = new Input("name")
        {
            Prompt = "Enter business name to search for",
            BoundingBox = { Top = 4 }
        };

        private Input capacity = new Input("capacity")
        {
            Prompt = "Enter new maximum capacity",
            BoundingBox = { Top = 5 }
        };

        private Label divider = new Label("divider")
        {
            Text = "----",
            BoundingBox = { Top = 6 }
        };

        private Button confirm = new Button("confirm")
        {
            Text = "[Confirm]",
            BoundingBox = { Top = 7 }
        };

        private Label result = new Label("result")
        {
            BoundingBox = { Top = 9 }
        };

        public EditCapacityScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {

        }

        [OnClick("confirm")] private void OnConfirm()
        {
            ChangeCapacity();
            businessName.ClearText();
            capacity.ClearText();
        }

        private void ChangeCapacity()
        {
            var targetBusiness = CovidManager.FindBusinessLocation(businessName.Text);
            if (targetBusiness != null)
            {
                var oldCapacity = targetBusiness.MaximumCapacity;
                targetBusiness.MaximumCapacity = Convert.ToInt32(capacity.Text);
                result.Text = $"Maximum capacity for {targetBusiness} has been changed from {oldCapacity} to {targetBusiness.MaximumCapacity}";
            }
            else
            {
                result.Text = $"{businessName.Text} not found. Maximum capacity has not been edited.";
            }
        }
    }
}
