using System;
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
            DisplayManager.RegisterScreen(new ListScreenBuilder<Visitor>(DisplayManager)
                .OfName("viewAllVisitors")
                .WithHeader("View All Visitors")
                .WithProperties(new[] {"Name", "PassportNo", "Nationality", "SafeEntryList", "TravelEntryList"})
                .WithGetter(() => Manager.GetAllPersonOfType<Visitor>())
                .Build()
            );

            DisplayManager.RegisterScreen(new ScreenBuilder(DisplayManager)
                .OfName("viewPersonDetails")
                .WithHeader("View Details of a Person")
                .AddElement(new Input("name", "Name"))
                .AddElement(new Spacer())
                .Build()
            );

            DisplayManager.RegisterScreen(new LegacyScreen(
                DisplayManager,
                "globalStats",
                "Explore Global Stats",
                ExploreGlobalStats
            ));
        }

        private void ExploreGlobalStats()
        {
            Console.WriteLine("ExploreGlobalStats");
        }
    }
}