using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.Utilities;

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
            var person = ConsoleHelper.GetInput("Enter name: ", Manager.FindPerson);
            if (person == null)
            {
                //TODO: Some message
                return;
            }

            var travelEntry = new TravelEntry(
                person,
                ConsoleHelper.GetInput("Last Country: "),
                ConsoleHelper.GetInput("Entry Mode: "),
                ConsoleHelper.GetInput("Entry Date: ", Convert.ToDateTime)
            );
            
            person.AddTravelEntry(travelEntry);
        }

        private void PaySHNCharges()
        {
            Console.WriteLine("PaySHNCharges");
        }

        private void GenerateSHNReport()
        {
            Console.WriteLine("GenerateSHNReport");


            DateTime n = ConsoleHelper.GetInput("Number: ", Convert.ToDateTime);
            Console.WriteLine(n);
        }
    }
}