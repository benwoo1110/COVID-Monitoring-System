//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.PersonMgr;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.TravelEntryMgr
{
    public class NewVisitorScreen : CovidScreen
    {
        public override string Name => "newVisitor";

        private Header header = new Header
        {
            Text = "New Visitor"
        };

        private Label info = new Label
        {
            Text = "Please Enter Details of the Visitor",
            BoundingBox = {Top = 4}
        };

        private Input name = new Input
        {
            Prompt = "Name",
            BoundingBox = {Top = 6},
        };

        private Input passportNo = new Input
        {
            Prompt = "Passport Number",
            BoundingBox = {Top = 7}
        };

        private Input nationality = new Input
        {
            Prompt = "Nationality",
            BoundingBox = {Top = 8},
            SuggestionType = "nationality"
        };

        private Button create = new Button
        {
            Text = "[Create]",
            BoundingBox = {Top = 10},
        };

        private Label result = new Label
        {
            BoundingBox = {Top = 12},
            ClearOnExit = true
        };

        public NewVisitorScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }

        [OnClick("create")]
        private void OnCreate(
            [InputParam("name", "result")] string nameText,
            [InputParam("passportNo", "result")] string passportNoText,
            [InputParam("nationality", "result")] string nationalityText)
        {
            if (CovidManager.AddPerson(new Visitor(nameText, passportNoText, nationalityText)))
            {
                result.Text = $"New visitor {nameText} has been added to the system.";
                ClearAllInputs();
                return;
            }
            result.Text = $"Visitor {nameText} was not added to the system. Is the passport number unique?";
        }
    }
}