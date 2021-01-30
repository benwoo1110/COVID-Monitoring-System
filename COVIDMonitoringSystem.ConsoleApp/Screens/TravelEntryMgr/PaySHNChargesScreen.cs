//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System.Collections.Generic;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.PersonMgr;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.TravelEntryMgr
{
    public class PaySHNChargesScreen : CovidScreen
    {
        public override string Name => "paySHNCharges";

        private Header header = new Header
        {
            Text = "Pay SHN Charges"
        };

        private Input name = new Input
        {
            Prompt = "Name",
            BoundingBox = {Top = 4}
        };

        private Button viewPayment = new Button
        {
            Text = "[View Payment]",
            BoundingBox = {Top = 6}
        };

        private Button paymentInfo = new Button
        {
            BoundingBox = {Top = 8},
            ClearOnExit = true
        };

        public PaySHNChargesScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }

        [OnClick("viewPayment")]
        private void OnPaymentView([InputParam("name", "paymentInfo")] Person targetPerson)
        {
            var payment = targetPerson.GenerateSHNPaymentDetails();
            if (!payment.HasUnpaidEntries())
            {
                paymentInfo.Text = $"{targetPerson.Name} does not make any unpaid travel entries.";
                return;
            }

            var paymentList = new List<string>();
            foreach (var entry in payment.Entries)
            {
                paymentList.Add($"{entry.LastCountryOfEmbarkation,-20} | ${entry.CalculateCharges():0.00}");
            }
            
            var detailsBuilder = new DetailsBuilder();
            detailsBuilder.AddLine($"{targetPerson.Name} has {payment.NumberOfUnpaidEntries()} unpaid travel entries.")
                .Separator()
                .AddOrderedList("Payment List", paymentList)
                .Separator()
                .AddLine($"Subtotal: ${payment.SubTotalPrice:0.00}")
                .AddLine($"Total: ${payment.TotalPrice:0.00} (include 7% GST)");

            paymentInfo.Text = detailsBuilder.Build();
        }

        /*private void PaySHNCharges()
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
        }*/
    }
}