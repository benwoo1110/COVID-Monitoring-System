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
            Console.WriteLine("View business locations");
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
