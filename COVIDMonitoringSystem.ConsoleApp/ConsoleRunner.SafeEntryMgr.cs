using System;
using System.Collections.Generic;
using System.Text;
using COVIDMonitoringSystem.ConsoleApp.Display;

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
            Console.WriteLine("Assign Token");
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
            Console.WriteLine("Change business location capacity");
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
