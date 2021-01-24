using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Builders;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.SafeEntryMgr;
using COVIDMonitoringSystem.Core.TravelEntryMgr;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public partial class ConsoleRunner
    {
        private void SetUpTravelEntryScreens()
        {
            DisplayManager.RegisterScreen(new ScreenBuilder(DisplayManager)
                .OfName("viewFacilities")
                .WithHeader("View All SHN Facilities")
                .AddElement(new ObjectList<BusinessLocation>(
                    new[] {"BusinessName", "BranchCode", "MaximumCapacity", "VisitorsNow"},
                    () => Manager.BusinessLocationList
                ))
                .Build()
            );

            DisplayManager.RegisterScreen(new ScreenBuilder(DisplayManager)
                .OfName("newVisitor")
                .WithHeader("Create New Visitor")
                .AddElement(new Input("Name"))
                .AddElement(new Input("Passport Number"))
                .AddElement(new Input("Nationality"))
                .AddElement(new Label())
                .Build()
            );

            DisplayManager.RegisterScreen(new LegacyScreen(
                DisplayManager,
                "travelRecord",
                "New Travel Record",
                NewTravelRecord
            ));

            DisplayManager.RegisterScreen(new LegacyScreen(
                DisplayManager,
                "paySHNCharges",
                "Pay SHN Charges",
                PaySHNCharges
            ));

            DisplayManager.RegisterScreen(new LegacyScreen(
                DisplayManager,
                "shnReport",
                "Generate SHN Status Report",
                GenerateSHNReport
            ));
        }

        private void NewTravelRecord()
        {
            var targetPerson = CHelper.GetInput("Enter name: ", Manager.FindPerson);
            if (targetPerson == null)
            {
                //TODO: Some message
                return;
            }

            var travelEntry = new TravelEntry(
                targetPerson,
                CHelper.GetInput("Last Country: "),
                CHelper.GetInput<TravelEntryMode>("Entry Mode: ", CHelper.EnumParser<TravelEntryMode>),
                CHelper.GetInput("Entry Date: ", Convert.ToDateTime)
            );
            
            //TODO: Add SHN Facility
            
            targetPerson.AddTravelEntry(travelEntry);
        }

        private void PaySHNCharges()
        {
            var targetPerson = CHelper.GetInput("Enter name: ", Manager.FindPerson);
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
            
            if (!CHelper.Confirm("Do you want to pay the travel entries now?"))
            {
                Console.WriteLine("No payment made, you can come again on a later date to do so.");
                return;
            }
            
            payment.DoPayment();
            Console.WriteLine($"Payment for {payment.NumberOfUnpaidEntries()} travel entries has be successfully made!");
        }

        private void GenerateSHNReport()
        {
            var targetDate = CHelper.GetInput("Enter Date to Report: ", Convert.ToDateTime);
            Console.WriteLine(Manager.GenerateSHNStatusReportFile(targetDate)
                ? "Successfully generated report file."
                : "There was an error generating report file.");
        }
    }
}