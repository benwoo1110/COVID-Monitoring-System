//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.PersonMgr;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.SafeEntryMgr
{
    public class TokenScreen : CovidScreen
    {
        public override string Name => "assignToken";

        private Header header = new Header("header")
        {
            Text = "Assign or replace TraceTogether Token",
            BoundingBox = { Top = 0 }
        };
        private Input name = new Input("name")
        {
            Prompt = "Enter resident name",
            BoundingBox = { Top = 4 }
        };
        private Button check = new Button("check")
        {
            Text = "[Check]",
            BoundingBox = {Top = 6}
        };
        private Label result = new Label("result")
        {
            BoundingBox = {Top = 8},
        };
        private Input location = new Input("location")
        {
            Prompt = "Enter your collection location",
            BoundingBox = {Top = 10},
            Hidden = true,
            Enabled = false
        };
        private Label output = new Label("output")
        {
            BoundingBox = { Top = 12 }
        };

        [OnClick("check")] private void OnCheck()
        {
            var targetResident = CovidManager.FindPersonOfType<Resident>(name.Text);
            if (targetResident != null)
            {
                AssignToken(targetResident);
            }
            else
            {
                result.Text = "Resident not found";
            }
        }
        
        private void AssignToken(Resident person)
        {
            if (person.Token == null)
            {
                location.Hidden = false;
                location.Enabled = true;
                result.Text = "A new token will be issued to you.";
                var generator = new Random();
                var serialNum = generator.Next(10000, 100000);
                var finalSerial = "T" + Convert.ToString(serialNum);
                var inputCollectDate = DateTime.Now;
                var expiry = inputCollectDate.AddMonths(6);
                var newT = new TraceTogetherToken(finalSerial, location.Text, expiry);
                person.Token = newT;
                output.Text = $"A new token has been issued to you. Your serial number is {finalSerial}, " +
                                    $"your collection location is at {location.Text} and the expiry date of your token is {expiry}.";
            }
            else if (person.Token.IsEligibleForReplacement())
            {
                result.Text = "Your token is expiring soon. A new token will be issued to you.";
                var generator = new Random();
                var serialNum = generator.Next(10000, 100000);
                var finalSerial = "T" + Convert.ToString(serialNum);
                location.Hidden = false;
                location.Enabled = true;
                person.Token.ReplaceToken(finalSerial, location.Text);
                CHelper.WriteLine($"A new token has been issued to you. Your serial number is {finalSerial}, " +
                                    $"your collection location is at {location.Text} and the expiry date of your token is " +
                                    $"{person.Token.ExpiryDate}.");
            }
            else
            {
                var expiry = person.Token.ExpiryDate;
                result.Text = $"Your token has not expired. The expiry date for your token is {expiry}";
            }

            if (person.Token == null)
            {
                result.Text = "Resident does not exist, or person is not a resident.";
            }  
        }

        public TokenScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }

    }
}
