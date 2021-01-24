using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Builders;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.SafeEntryMgr;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public partial class ConsoleRunner
    {
        private void SetUpSafeEntryScreens()
        {
            DisplayManager.RegisterScreen(new LegacyScreen(
                DisplayManager,
                "assignToken",
                "Assign or replace TraceTogether Token",
                AssignToken
            ));

            DisplayManager.RegisterScreen(new ScreenBuilder(DisplayManager)
                .OfName("viewLocations")
                .WithHeader("View All Visitors")
                .AddElement(new ObjectList<BusinessLocation>(
                    new[] {"BusinessName", "BranchCode", "MaximumCapacity", "VisitorsNow"},
                    () => Manager.BusinessLocationList
                ))
                .Build()
            );

            DisplayManager.RegisterScreen(new LegacyScreen(
                DisplayManager,
                "changeCapacity",
                "Change capacity of business location",
                ChangeCapacity
            ));

            DisplayManager.RegisterScreen(new LegacyScreen(
                DisplayManager,
                "checkIn",
                "SafeEntry Check-In",
                CheckIn
            ));

            DisplayManager.RegisterScreen(new LegacyScreen(
                DisplayManager,
                "checkOut",
                "SafeEntry Check-Out",
                CheckOut
            ));
        }
        
        private void AssignToken()
        {
            Resident targetResident = CHelper.GetInput("Enter resident name: ", Manager.FindPersonOfType<Resident>);
            if (targetResident != null)
            {
                if (targetResident.Token == null)
                {
                    Console.WriteLine("A new token will be issued to you.");
                    Random generator = new Random();
                    int serialnum = generator.Next(10000, 100000);
                    var finalSerial = "T" + Convert.ToString(serialnum);
                    var inputCollectLocation = CHelper.GetInput("Enter your collection location: ");
                    var inputCollectDate = DateTime.Now;
                    var expiry = inputCollectDate.AddMonths(6);
                    TraceTogetherToken newT = new TraceTogetherToken(finalSerial, inputCollectLocation, expiry);
                    targetResident.Token = newT;
                    Console.WriteLine($"A new token has been issued to you. Your serial number is {finalSerial}, " +
                        $"your collection location is at {inputCollectLocation} and the expiry date of your token is {expiry}.");
                }
                else if (targetResident.Token.IsEligibleForReplacement())
                {
                    Console.WriteLine("Your token is expiring soon. A new token will be issued to you.");
                    Random generator = new Random();
                    var serialnum = generator.Next(10000, 100000);
                    var finalSerial = "T" + Convert.ToString(serialnum);
                    var inputCollectLocation = CHelper.GetInput("Enter your collection location: ");
                    targetResident.Token.ReplaceToken(finalSerial, inputCollectLocation);
                    Console.WriteLine($"A new token has been issued to you. Your serial number is {finalSerial}, " +
                        $"your collection location is at {inputCollectLocation} and the expiry date of your token is " +
                        $"{targetResident.Token.ExpiryDate}.");
                }
                else
                {
                    Console.WriteLine("Your token has not expired. Dumbass");
                }
                return;
            }
            Console.WriteLine("Resident does not exist, or person is not a resident.");
            
        }

        private void ChangeCapacity()
        {
            var targetBusiness = CHelper.GetInput("Enter business name to search for: ", Manager.FindBusinessLocation);
            if (targetBusiness != null)
            {
                int newCapacity = CHelper.GetInput("Enter new maximum capacity: ", Convert.ToInt32);
                targetBusiness.MaximumCapacity = newCapacity;
            }
            else
            {
                Console.WriteLine("Business not found. Dumbass");
            }
        }

        private void CheckIn()
        {
            Console.WriteLine("Check In for SafeEntry");
        }

        private void CheckOut()
        {
            Console.WriteLine("Check Out for SafeEntry");
        }
    }

}
