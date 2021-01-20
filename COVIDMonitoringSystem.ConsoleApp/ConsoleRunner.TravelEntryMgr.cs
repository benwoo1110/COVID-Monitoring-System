using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core;

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
            Manager.AddPerson(new Visitor(
                ConsoleHelper.GetInput("Enter Name: "),
                ConsoleHelper.GetInput("Enter Passport Number: "),
                ConsoleHelper.GetInput("Enter Nationality: ")
            ));
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