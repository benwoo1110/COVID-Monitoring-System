using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.TravelEntryMgr;

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
            FancyObjectDisplay.PrintList(Manager.SHNFacilitiesList);
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
            var targetPerson = ConsoleHelper.GetInput("Enter name: ", Manager.FindPerson);
            if (targetPerson == null)
            {
                //TODO: Some message
                return;
            }

            var travelEntry = new TravelEntry(
                targetPerson,
                ConsoleHelper.GetInput("Last Country: "),
                ConsoleHelper.GetInput("Entry Mode: ", ConsoleHelper.EnumParser<TravelEntryMode>),
                ConsoleHelper.GetInput("Entry Date: ", Convert.ToDateTime)
            );
            
            targetPerson.AddTravelEntry(travelEntry);
        }

        private void PaySHNCharges()
        {
            var targetPerson = ConsoleHelper.GetInput("Enter name: ", Manager.FindPerson);
            if (targetPerson == null)
            {
                //TODO: Some message
                return;
            }

            var payment = targetPerson.GenerateSHNPaymentDetails();
            if (!payment.HasUnpaidEntries())
            {
                Console.WriteLine($"{targetPerson.Name} does not make any unpaid travel entries.");
                return;
            }
            
            Console.WriteLine($"{targetPerson.Name} has {payment.NumberOfUnpaidEntries()} unpaid travel entries.");
            var index = 1;
            foreach (var entry in payment.Entries)
            {
                Console.WriteLine($"{index} - {entry.LastCountryOfEmbarkation,-20} | ${entry.CalculateCharges():0.00}");
                index++;
            }
            Console.WriteLine($"Subtotal: ${payment.SubTotalPrice:0.00}");
            Console.WriteLine($"Total: ${payment.TotalPrice:0.00} (include 7% GST)");
            
            if (!ConsoleHelper.Confirm("Do you want to pay the travel entries now?"))
            {
                Console.WriteLine("No payment made, you can come again on a later date to do so.");
                return;
            }
            
            payment.DoPayment();
            Console.WriteLine($"Payment for {payment.NumberOfUnpaidEntries()} travel entries has be successfully made!");
        }

        private void GenerateSHNReport()
        {
            Console.WriteLine("GenerateSHNReport");


            DateTime n = ConsoleHelper.GetInput("Number: ", Convert.ToDateTime);
            Console.WriteLine(n);
        }
    }
}