using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.TravelEntryMgr;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.TravelEntryMgr
{
    public class PaySHNChargesScreen : CovidScreen
    {
        public override string Name => "paySHNCharges";
        
        private Header header = new Header
        {
            Text = "View/Pay SHN Charges"
        };
        
        private Label paymentInfo = new Label
        {
            BoundingBox = {Top = 4}
        };

        private Input payAmount = new Input
        {
            Prompt = "Amount ($)",
            BoundingBox = {Top = 0}
        };

        private Button pay = new Button
        {
            Text = "[Pay]",
            BoundingBox = {Top = 2}
        };
        
        private Label result = new Label
        {
            BoundingBox = {Top = 4}
        };
        
        private Button back = new Button
        {
            Text = "[Back]",
            BoundingBox = {Top = 0}
        };

        private SHNPayment Payment { get; set; }
        private string PaymentDetails { get; set; }

        public PaySHNChargesScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
            payAmount.BoundingBox.SetRelativeElement(paymentInfo);
            pay.BoundingBox.SetRelativeElement(paymentInfo);
            result.BoundingBox.SetRelativeElement(paymentInfo);
            back.BoundingBox.SetRelativeElement(result);
        }

        public override void PrePassData(object data)
        {
            try
            {
                var dataArray = (object[]) data;
                Payment = (SHNPayment) dataArray[0];
                PaymentDetails = (string) dataArray[1];
            }
            catch (Exception)
            {
                Payment = null;
                PaymentDetails = null;
            }
        }

        public override void PreLoad()
        {
            paymentInfo.Text = PaymentDetails;
            back.Hidden = true;

            payAmount.Enabled = true;
            pay.Enabled = true;
        }

        [OnClick("pay")]
        private void OnDoPayment(
            [InputParam("payAmount", "result")] double amount)
        {
            var change = Payment.DoPayment(amount);
            if (change < 0)
            {
                result.Text = "Insufficient amount. Payment is not done.";
                return;
            }

            result.Text = $"Payment completed. You receive back change of ${change:0.00}.";
            back.Hidden = false;
            
            payAmount.Enabled = false;
            pay.Enabled = false;
        }

        [OnClick("back")]
        private void OnBack()
        {
            DisplayManager.PopScreen();
        }
    }
}