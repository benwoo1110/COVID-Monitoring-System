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

        private Label output = new Label("output")
        {
            BoundingBox = { Top = 8 }
        };

        private void AssignToken()
        {
            var targetResident = CovidManager.FindPersonOfType<Resident>(name.Text);
            if (targetResident != null)
            {
                if (targetResident.Token == null)
                {
                    output.Text = "A new token will be issued to you.";
                    var generator = new Random();
                    var serialNum = generator.Next(10000, 100000);
                    var finalSerial = "T" + Convert.ToString(serialNum);
                    var inputCollectLocation = CHelper.GetInput("Enter your collection location: ");
                    var inputCollectDate = DateTime.Now;
                    var expiry = inputCollectDate.AddMonths(6);
                    var newT = new TraceTogetherToken(finalSerial, inputCollectLocation, expiry);
                    targetResident.Token = newT;
                    CHelper.WriteLine($"A new token has been issued to you. Your serial number is {finalSerial}, " +
                                      $"your collection location is at {inputCollectLocation} and the expiry date of your token is {expiry}.");
                }
                else if (targetResident.Token.IsEligibleForReplacement())
                {
                    CHelper.WriteLine("Your token is expiring soon. A new token will be issued to you.");
                    var generator = new Random();
                    var serialNum = generator.Next(10000, 100000);
                    var finalSerial = "T" + Convert.ToString(serialNum);
                    var inputCollectLocation = CHelper.GetInput("Enter your collection location: ");
                    targetResident.Token.ReplaceToken(finalSerial, inputCollectLocation);
                    CHelper.WriteLine($"A new token has been issued to you. Your serial number is {finalSerial}, " +
                                      $"your collection location is at {inputCollectLocation} and the expiry date of your token is " +
                                      $"{targetResident.Token.ExpiryDate}.");
                }
                else
                {
                    CHelper.WriteLine("Your token has not expired. Sorry ;(");
                }
                return;
            }

            CHelper.WriteLine("Resident does not exist, or person is not a resident.");
        }

        public TokenScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {

        }

        [OnEnterInput("name")] private void OnToken()
        {
            AssignToken();

            // remove input field and replace with the function?
            // exit the process and go back to menu automatically?
        }
    }
}
