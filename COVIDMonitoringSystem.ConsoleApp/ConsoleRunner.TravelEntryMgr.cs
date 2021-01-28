using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Builders;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.ConsoleApp.Screens;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.TravelEntryMgr;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public partial class ConsoleRunner
    {
        private void SetUpTravelEntryScreens()
        {
            DisplayManager.RegisterScreen(new ListScreenBuilder<SHNFacility>(DisplayManager)
                .OfName("viewFacilities")
                .WithHeader("View All SHN Facilities")
                .WithProperties(new[] {"FacilityName", "FacilityCapacity", "FacilityVacancy", "FromLand", "FromSea", "FromAir"})
                .WithGetter(() => Manager.SHNFacilitiesList)
                .Build()
            );
            
            DisplayManager.RegisterScreen(new NewVisitorScreen(DisplayManager));

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

        private void CreateNewVisitor(Screen screen)
        {
            var name = screen.FindElementOfType<Input>("name");
            var passportNo = screen.FindElementOfType<Input>("passportNo");
            var nationality = screen.FindElementOfType<Input>("nationality");
            var result = screen.FindElementOfType<TextElement>("result");

            if (!name.HasText() || !passportNo.HasText() || !nationality.HasText())
            {
                result.Text = "Incomplete details. No visitor has been added to the system.";
                return;
            }
            
            Manager.AddPerson(new Visitor(name.Text, passportNo.Text, nationality.Text));
            result.Text = $"New visitor {name.Text} has been added to the system.";

            name.ClearText();
            passportNo.ClearText();
            nationality.ClearText();
        }

        private void NewTravelRecord()
        {
            var targetPerson = CHelper.GetInput("Enter name: ", Manager.FindPerson);
            if (targetPerson == null)
            {
                CHelper.WriteLine("No such person found!");
                return;
            }

            var travelEntry = new TravelEntry(
                targetPerson,
                CHelper.GetInput("Last Country: "),
                CHelper.GetInput("Entry Mode: ", CHelper.EnumParser<TravelEntryMode>),
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
                CHelper.WriteLine("No such person found!");
                return;
            }

            var payment = targetPerson.GenerateSHNPaymentDetails();
            if (!payment.HasUnpaidEntries())
            {
                CHelper.WriteLine($"{targetPerson.Name} does not make any unpaid travel entries.");
                return;
            }

            CHelper.WriteLine($"{targetPerson.Name} has {payment.NumberOfUnpaidEntries()} unpaid travel entries.");
            var index = 1;
            foreach (var entry in payment.Entries)
            {
                CHelper.WriteLine($"{index} - {entry.LastCountryOfEmbarkation,-20} | ${entry.CalculateCharges():0.00}");
                index++;
            }

            CHelper.WriteLine($"Subtotal: ${payment.SubTotalPrice:0.00}");
            CHelper.WriteLine($"Total: ${payment.TotalPrice:0.00} (include 7% GST)");

            if (!CHelper.Confirm("Do you want to pay the travel entries now?"))
            {
                CHelper.WriteLine("No payment made, you can come again on a later date to do so.");
                return;
            }

            payment.DoPayment();
            CHelper.WriteLine($"Payment for {payment.NumberOfUnpaidEntries()} travel entries has be successfully made!");
        }

        private void GenerateSHNReport()
        {
            var targetDate = CHelper.GetInput("Enter Date to Report: ", Convert.ToDateTime);
            CHelper.WriteLine(Manager.GenerateSHNStatusReportFile(targetDate)
                ? "Successfully generated report file."
                : "There was an error generating report file.");
        }
    }
}