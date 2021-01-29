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

        private Header header = new Header("header")
        {
            Text = "New Visitor"
        };
        private Label info = new Label("info")
        {
            Text = "Please Enter Details of the Visitor",
            BoundingBox = {Top = 4}
        };
        private Input name = new Input("name")
        {
            Prompt = "Name",
            BoundingBox = {Top = 6}
        };
        private Input passportNo = new Input("passportNo")
        {
            Prompt = "Passport Number",
            BoundingBox = {Top = 7}
        };
        private Input nationality = new Input("Nationality")
        {
            Prompt = "Nationality",
            BoundingBox = {Top = 8}
        };
        private Button create = new Button("create")
        {
            Text = "[Create]",
            BoundingBox = {Top = 10},
        };
        private Label result = new Label("result")
        {
            BoundingBox = {Top = 12}
        };

        public NewVisitorScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }

        [OnClick("create")] private void OnCreate()
        {
            if (!name.HasText() || !passportNo.HasText() || !nationality.HasText())
            {
                result.Text = "Incomplete details. No visitor has been added to the system.";
                return;
            }

            CovidManager.AddPerson(new Visitor(name.Text, passportNo.Text, nationality.Text));
            result.Text = $"New visitor {name.Text} has been added to the system.";

            name.ClearText();
            passportNo.ClearText();
            nationality.ClearText();
        }
    }
}