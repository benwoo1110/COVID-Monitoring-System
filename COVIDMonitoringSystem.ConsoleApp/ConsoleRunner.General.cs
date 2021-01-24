using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Builders;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core.PersonMgr;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public partial class ConsoleRunner
    {
        private void SetUpGeneralScreens()
        {
            DisplayManager.RegisterScreen(new ScreenBuilder(DisplayManager)
                .OfName("viewAllVisitors")
                .WithHeader("View All Visitors")
                .AddElement(new ObjectList<Visitor>(
                    new[] {"Name", "PassportNo", "Nationality", "SafeEntryList", "TravelEntryList"},
                    () => Manager.GetAllPersonOfType<Visitor>()
                ))
                .Build()
            );

            DisplayManager.RegisterScreen(new ScreenBuilder(DisplayManager)
                .OfName("viewPersonDetails")
                .WithHeader("View Details of a Person")
                .AddElement(new Input("Name"))
                .AddElement(new Label())
                .Build()
            );

            DisplayManager.RegisterScreen(new LegacyScreen(
                DisplayManager,
                "globalStats",
                "Explore Global Stats",
                ExploreGlobalStats
            ));
        }
    }
}