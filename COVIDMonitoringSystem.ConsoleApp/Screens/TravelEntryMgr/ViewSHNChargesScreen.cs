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
using COVIDMonitoringSystem.Core.TravelEntryMgr;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.TravelEntryMgr
{
    public class ViewSHNChargesScreen : CovidScreen
    {
        public override string Name => "viewSHNCharges";

        private Header header = new Header
        {
            Text = "View/Pay SHN Charges"
        };

        private Input name = new Input
        {
            Prompt = "Name",
            BoundingBox = {Top = 4},
            SuggestionType = "person"
        };

        private Button viewPayment = new Button
        {
            Text = "[View Payment]",
            BoundingBox = {Top = 6}
        };

        private Label paymentInfo = new Label
        {
            BoundingBox = {Top = 8},
            ClearOnExit = true
        };

        private Button payNow = new Button
        {
            Text = "[Pay Now]",
            BoundingBox = {Top = 0},
        };

        public SHNPayment CachedPayment { get; set; }

        public ViewSHNChargesScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
            payNow.BoundingBox.SetRelativeElement(paymentInfo);
        }

        public override void PreLoad()
        {
            payNow.Hidden = true;
        }

        [OnClick("viewPayment")]
        private void OnPaymentView([InputParam("name", "paymentInfo")] Person targetPerson)
        {
            CachedPayment = targetPerson.GenerateSHNPaymentDetails();
            if (!CachedPayment.HasUnpaidEntries())
            {
                paymentInfo.Text = $"{targetPerson.Name} does not make any unpaid travel entries.";
                payNow.Hidden = true;
                return;
            }

            var paymentList = new List<string>();
            foreach (var entry in CachedPayment.Entries)
            {
                paymentList.Add($"{entry.LastCountryOfEmbarkation,-20} | ${entry.CalculateCharges():0.00}");
            }
            
            var detailsBuilder = new DetailsBuilder();
            detailsBuilder.AddLine($"{targetPerson.Name} has {CachedPayment.NumberOfUnpaidEntries()} unpaid travel entries.")
                .Separator()
                .AddOrderedList("Payment List", paymentList)
                .Separator()
                .AddLine($"Subtotal: ${CachedPayment.SubTotalPrice:0.00}")
                .AddLine($"Total: ${CachedPayment.TotalPrice:0.00} (include 7% GST)");

            paymentInfo.Text = detailsBuilder.Build();
            payNow.Hidden = false;
        }

        [OnClick("payNow")]
        private void OnPayNow()
        {
            DisplayManager.PushScreen("paySHNCharges", new object[] {CachedPayment, paymentInfo.Text});
        }
    }
}