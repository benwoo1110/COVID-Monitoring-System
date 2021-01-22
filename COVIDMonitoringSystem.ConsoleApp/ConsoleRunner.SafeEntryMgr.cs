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
            MenusCollection["safe-entry"].RunMenuOption();
        }
        private void AssignToken()
        {
            var targetPerson = ConsoleHelper.GetInput("Enter person's name: ", Manager.FindPerson);

        }

        private void ViewLocations()
        {
            FancyObjectDisplay.PrintHeader();
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
