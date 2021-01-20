using System;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public partial class ConsoleRunner
    {
        private void ManageTravelEntry()
        {
            MenusCollection["travelentry"].RunMenuOption();
        }

        private void ViewAllSHNFacility()
        {
            Console.WriteLine("ViewAllSHNFacility");
        }

        private void NewVisitor()
        {
            Console.WriteLine("NewVisitor");
        }

        private void NewTravelRecord()
        {
            Console.WriteLine("NewTravelRecord");
        }

        private void PaySHNCharges()
        {
            Console.WriteLine("PaySHNCharges");
        }

        private void GenerateSHNReport()
        {
            Console.WriteLine("GenerateSHNReport");
        }
    }
}