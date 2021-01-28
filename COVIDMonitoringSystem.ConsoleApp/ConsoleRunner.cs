using COVIDMonitoringSystem.ConsoleApp.Builders;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.Core;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public partial class ConsoleRunner
    {
        private COVIDMonitoringManager Manager { get; }

        private ConsoleDisplayManager DisplayDisplayManager { get; }

        public ConsoleRunner()
        {
            DisplayDisplayManager = new ConsoleDisplayManager();
            Manager = new COVIDMonitoringManager();
            SetUpMenus();
        }

        private void SetUpMenus()
        {
            DisplayDisplayManager.RegisterScreen(new MainMenuBuilder(DisplayDisplayManager)
                .OfName("mainMenu")
                .WithHeader("COVID Management System")
                .AddOption("View Details of a Person", "viewPersonDetails")
                .AddOption("View All Visitors", "viewAllVisitors")
                .AddOption("Safe Entry Management", "safeEntryMenu")
                .AddOption("Travel Entry Management", "travelEntryMenu")
                .AddOption("Explore Global Stats", "globalStats")
                .Build()
            );

            DisplayDisplayManager.RegisterScreen(new MenuBuilder(DisplayDisplayManager)
                .OfName("safeEntryMenu")
                .WithHeader("Travel Entry Management")
                .AddOption("Assign or replace TraceTogether Token", "assignToken")
                .AddOption("View all business locations", "viewLocations")
                .AddOption("Change capacity of business location", "changeCapacity")
                .AddOption("SafeEntry Check-In", "checkIn")
                .AddOption("SafeEntry Check-Out", "checkOut")
                .AddOption("Generate Contact Tracing Report", "contactTrace")
                .Build()
            );

            DisplayDisplayManager.RegisterScreen(new MenuBuilder(DisplayDisplayManager)
                .OfName("travelEntryMenu")
                .WithHeader("Travel Entry Management")
                .AddOption("View All SHN Facilities", "viewFacilities")
                .AddOption("New Visitor", "newVisitor")
                .AddOption("New Travel Record", "travelRecord")
                .AddOption("Pay SHN Charges", "paySHNCharges")
                .AddOption("Generate SHN Status Report", "shnReport")
                .Build()
            );

            SetUpGeneralScreens();
            SetUpSafeEntryScreens();
            SetUpTravelEntryScreens();
        }

        public void Run()
        {
            DisplayDisplayManager.Run("mainMenu");
        }
    }
}