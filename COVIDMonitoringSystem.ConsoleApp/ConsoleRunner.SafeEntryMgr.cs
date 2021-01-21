using System;
using System.Collections.Generic;
using System.Text;

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
            Console.WriteLine("{0,10} {1,10} {2,10}", "Business Name", "Branch Code", "Max Capacity");
            foreach (var b in Manager.BusinessLocationList)
            {
                Console.WriteLine("{0,10} {1,10} {2,10}", b.BusinessName, b.BranchCode, b.MaximumCapacity);
            }
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
