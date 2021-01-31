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

        private Header header = new Header
        {
            Text = "Assign or replace TraceTogether Token",
            BoundingBox = { Top = 0 }
        };
        private Input name = new Input
        {
            Prompt = "Enter resident name",
            BoundingBox = { Top = 4 },
            SuggestionType = "resident"
        };
        private Button check = new Button
        {
            Text = "[Check]",
            BoundingBox = {Top = 6}
        };
        private Label result = new Label
        {
            BoundingBox = {Top = 8},
            ClearOnExit = true
        };
        private Input location = new Input
        {
            Prompt = "Enter your collection location",
            BoundingBox = {Top = 10},
            Hidden = true,
            Enabled = false,
            SuggestionType = "collectLocation"
        };
        private Button getToken = new Button
        {
            Text = "[Get Token]",
            BoundingBox = {Top = 11},
            Hidden = true,
            Enabled = false
        };
        private Label output = new Label
        {
            BoundingBox = {Top = 13},
            ClearOnExit = true
        };

        public TokenScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }

        public override void OnClose()
        {
            location.Hidden = true;
            location.Enabled = false;
            getToken.Hidden = true;
            getToken.Enabled = false;
        }

        [OnClick("check")] private void OnCheck(
            [InputParam("name", "result")] Resident targetResident)
        {
            output.ClearText();
            targetResident = CovidManager.FindPersonOfType<Resident>(name.Text);
            if (targetResident != null)
            {
                AssignToken(targetResident);
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
                getToken.Hidden = false;
                getToken.Enabled = true;
                result.Text = "A new token will be issued to you.";
                var generator = new Random();
                var serialNum = generator.Next(10000, 100000);
                var finalSerial = "T" + Convert.ToString(serialNum);
                while (!AcceptableToken(finalSerial))
                {
                    finalSerial = "T" + Convert.ToString(serialNum);
                }
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
                while (!AcceptableToken(finalSerial))
                {
                    finalSerial = "T" + Convert.ToString(serialNum);
                }
                location.Hidden = false;
                location.Enabled = true;
                getToken.Hidden = false;
                getToken.Enabled = true;
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

        private void DisplayToken(string serialno, string collection, DateTime expiry)
        {
            output.Text = $"A new token has been issued to you. Your token's serial number is {serialno}, " +
                $"your collection location is {collection} and your token expires on {expiry}.";
            ClearAllInputs();
            result.ClearText();
            location.Hidden = true;
            location.Enabled = false;
            getToken.Hidden = true;
            getToken.Enabled = false;
        }

        private bool AcceptableToken(string serialno)
        {
            foreach (Resident r in CovidManager.PersonList)
            {
                if (r.Token.SerialNo == serialno)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
    }
}
