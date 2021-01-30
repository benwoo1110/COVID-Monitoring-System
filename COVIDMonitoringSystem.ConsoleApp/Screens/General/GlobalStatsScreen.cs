//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core;

namespace COVIDMonitoringSystem.ConsoleApp.Screens.General
{
    public class GlobalStatsScreen : CovidScreen
    {
        public override string Name => "globalStats";

        private Header header = new Header("header")
        {
            Text = "Globel Stats",
            BoundingBox = {Top = 0}
        };

        private Input country = new Input("country")
        {
            Prompt = "Enter a Country",
            BoundingBox = {Top = 4}
        };

        private Button searchCountry = new Button("searchCountry")
        {
            Text = "[Search Country]",
            BoundingBox = {Top = 6}
        };

        private Label result = new Label("result")
        {
            Text = "Search for a country and see what happens!",
            BoundingBox = {Top = 8}
        };

        public GlobalStatsScreen(ConsoleDisplayManager displayManager, COVIDMonitoringManager covidManager) : base(displayManager, covidManager)
        {
        }

        [OnClick("searchCountry")]
        private void OnSearchCountry(
            [InputParam("country", "result")] string countryStuff)
        {
            var targetCountry = country.Text;
            result.Text = $"Search for {countryStuff}!";
            country.ClearText();
        }
    }
}