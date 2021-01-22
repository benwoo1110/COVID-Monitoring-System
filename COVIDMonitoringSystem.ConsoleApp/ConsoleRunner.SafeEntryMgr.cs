using System;
using System.Collections.Generic;
using System.Text;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core.PersonMgr;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public partial class ConsoleRunner
    {
        private void ManageSafeEntry()
        {
            MenusCollection["safeEntry"].RunMenuOption();
        }
        private void AssignToken()
        {
            Resident targetResident = ConsoleHelper.GetInput("Enter resident name: ", Manager.FindPersonOfType<Resident>);
            if (targetResident != null)
            {
                if (targetResident.Token == null)
                {
                    Console.WriteLine("A new token will be issued to you.");
                    Random generator = new Random();
                    int serialnum = generator.Next(10000, 100000);
                    var finalSerial = "T" + Convert.ToString(serialnum);
                    var inputCollectLocation = ConsoleHelper.GetInput("Enter your collection location: ");
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
                    var inputCollectLocation = ConsoleHelper.GetInput("Enter your collection location: ");
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

        private void ViewLocations()
        {
            FancyObjectDisplay.PrintList(
                Manager.BusinessLocationList,
                new []{ "BusinessName", "BranchCode", "MaximumCapacity", "VisitorsNow" }    
            );
        }

        private void ChangeCapacity()
        {
            var targetBusiness = ConsoleHelper.GetInput("Enter business name to search for: ", Manager.FindBusinessLocation);
            if (targetBusiness != null)
            {
                int newCapacity = ConsoleHelper.GetInput("Enter new maximum capacity: ", Convert.ToInt32);
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
