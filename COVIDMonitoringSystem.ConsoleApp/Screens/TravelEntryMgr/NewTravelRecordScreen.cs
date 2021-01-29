using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.TravelEntryMgr
{
    public class NewTravelRecordScreen : CovidScreen
    {
        public override string Name => "newTravelRecord";

        private Header header = new Header("header")
        {
            Text = "New Travel Record",
            BoundingBox = {Top = 0}
        };
        private Input name = new Input("name")
        {
            Prompt = "Name",
            BoundingBox = {Top = 4}
        };
        private Input country = new Input("country")
        {
            Prompt = "Last Country of Embark",
            BoundingBox = {Top = 5}
        };
        private Input entryMode = new Input("entryMode")
        {
            Prompt = "Entry Mode",
            BoundingBox = {Top = 6}
        };
        private Input entryDate = new Input("entryDate")
        {
            Prompt = "Entry Date",
            BoundingBox = {Top = 7}
        };
        private Label shnMessage = new Label("shnMessage")
        {
            Text = "Some message",
            BoundingBox = {Top = 9},
            Hidden = true
        };
        private Input shnFacility = new Input("shnFacility")
        {
            Prompt = "SHN Facility Name",
            BoundingBox = {Top = 0},
            Hidden = true
        };

        private Button create = new Button("create")
        {
            Text = "[Create Record]",
            BoundingBox = {Top = 1}
        };
        
        public NewTravelRecordScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
            shnFacility.BoundingBox.SetRelativeBox(shnMessage);
            create.BoundingBox.SetRelativeBox(shnFacility);
        }
    }
}