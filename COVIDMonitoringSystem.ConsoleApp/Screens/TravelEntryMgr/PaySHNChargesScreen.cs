//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.TravelEntryMgr
{
    public class PaySHNChargesScreen : CovidScreen
    {
        public override string Name => "paySHNCharges";

        private Header header = new Header("header")
        {
            Text = "Pay SHN Charges"
        };
        private Input name = new Input("name")
        {
            Prompt = "Name",
            BoundingBox = {Top = 4}
        };
        private Button viewPayment = new Button("viewPayment")
        {
            Text = "[View Payment]",
            BoundingBox = {Top = 6}
        };
        private Button paymentInfo = new Button("paymentInfo")
        {
            BoundingBox = {Top = 8}
        };
        
        public PaySHNChargesScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }
    }
}