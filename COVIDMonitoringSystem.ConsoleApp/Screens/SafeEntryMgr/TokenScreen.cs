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
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.SafeEntryMgr;

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
        private Button getToken = new Button("getToken")
        {
            Text = "[Get Token]",
            BoundingBox = { Top = 11 }
        };
        private Label output = new Label("output")
        {
            BoundingBox = { Top = 13 }
        };

        public TokenScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }

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

        [OnClick("getToken")] private void OnToken()
        {
            var targetResident = CovidManager.FindPersonOfType<Resident>(name.Text);
            var finalSerial = targetResident.Token.SerialNo;
            var collectionLocation = location.Text;
            var expiry = targetResident.Token.ExpiryDate;
            DisplayToken(finalSerial, collectionLocation, expiry);
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

        private void DisplayToken(string serialno, string location, DateTime expiry)
        {
            output.Text = $"A new token has been issued to you. Your token's serial number is {serialno}, " +
                $"your collection location is {location} and your token expires on {expiry}.";
        }
    }
}
