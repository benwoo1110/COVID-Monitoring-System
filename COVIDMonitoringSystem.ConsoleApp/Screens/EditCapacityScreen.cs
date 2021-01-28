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
    public class EditCapacityScreen : CovidScreen
    {
        public override string Name => "changeCapacity";

        private Header header = new Header("header")
        {
            Text = "Change capacity of business location",
            BoundingBox = { Top = 0 }
        };

        private Input name = new Input("name")
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

        private void ChangeCapacity()
        {
            var targetBusiness = CovidManager.FindBusinessLocation(name.Text);
            var oldCapacity = targetBusiness.MaximumCapacity;
            if (targetBusiness != null)
            {
                targetBusiness.MaximumCapacity = Convert.ToInt32(capacity); // Unable to convert like this, how else can we convert user input?
                result.Text = $"Maximum capacity for {targetBusiness} has been changed from {oldCapacity} to {targetBusiness.MaximumCapacity}";
            }
            else
            {
                result.Text = $"{targetBusiness} not found. Maximum capacity has not been edited.";
            }
        }
        [OnClick("confirm")]private void OnConfirm()
        {
            ChangeCapacity();
            name.ClearText();
            capacity.ClearText();
        }

        public EditCapacityScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {

        }
    }
}
