﻿using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Builders;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
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

            DisplayManager.RegisterScreen(new ListScreenBuilder<BusinessLocation>(DisplayManager)
                .OfName("viewLocations")
                .WithHeader("View All Visitors")
                .WithProperties(new[] {"BusinessName", "BranchCode", "MaximumCapacity", "VisitorsNow"})
                .WithGetter(() => Manager.BusinessLocationList)
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
            var targetResident = CHelper.GetInput("Enter resident name: ", Manager.FindPersonOfType<Resident>);
            if (targetResident != null)
            {
                if (targetResident.Token == null)
                {
                    CHelper.WriteLine("A new token will be issued to you.");
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

        private void ChangeCapacity()
        {
            var targetBusiness = CHelper.GetInput("Enter business name to search for: ", Manager.FindBusinessLocation);
            if (targetBusiness != null)
            {
                var newCapacity = CHelper.GetInput("Enter new maximum capacity: ", Convert.ToInt32);
                targetBusiness.MaximumCapacity = newCapacity;
            }
            else
            {
                CHelper.WriteLine("Business not found. Sorry ;(");
            }
        }

        private void CheckIn()
        {
            /*var inputName = CHelper.GetInput("Enter your name: ", Manager.FindPerson);
            if (inputName != null)
            {
                var inputLocation = CHelper.GetInput("Enter business location to check in to: ", Manager.FindBusinessLocation);
                if (inputLocation.IsFull())
                {
                    CHelper.WriteLine("This store is full, please try again later.");
                }
                else
                {
                    var checkIn = new SafeEntry(DateTime.Now, inputLocation);
                    var personEntry = new Person()
                    inputLocation.VisitorsNow += 1;
                }
                return;
            }
            else
            {
                CHelper.WriteLine("Name is not found, please try again.");
            }*/
            CHelper.WriteLine("Check In");
        }

        private void CheckOut()
        {
            /*var inputName = CHelper.GetInput("Enter your name: ", Manager.FindPerson);
            if (inputName != null)
            {

            }*/
            CHelper.WriteLine("Check out");
        }
    }

}
