//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using COVIDMonitoringSystem.ConsoleApp.Builders;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.TravelEntryMgr;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public partial class ConsoleRunner
    {
        private COVIDMonitoringManager Manager { get; }

        private ConsoleDisplayManager DisplayManager { get; }

        public ConsoleRunner()
        {
            DisplayManager = new ConsoleDisplayManager();
            Manager = new COVIDMonitoringManager();
            RegisterInputResolvers();
            SetUpMenus();
            SetUpGeneralScreens();
            SetUpSafeEntryScreens();
            SetUpTravelEntryScreens();
        }

        private void RegisterInputResolvers()
        {
            DisplayManager.ResolveManager.RegisterQuickInputResolver(
                Manager.FindPerson,
                "No person with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterQuickInputResolver(
                Manager.FindPersonOfType<Resident>,
                "No resident with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterQuickInputResolver(
                Manager.FindPersonOfType<Visitor>,
                "No visitor with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterQuickInputResolver(
                Manager.FindBusinessLocation,
                "No business with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterQuickInputResolver(
                Manager.FindSHNFacility,
                "No SHN facility with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterQuickInputResolver(
                SHNTier.FindAppropriateTier,
                "This shouldn't happen. Input was: {0}"
            );
            DisplayManager.ResolveManager.RegisterInputResolver<TravelEntryMode>(
                (screen, value) => CHelper.EnumParser<TravelEntryMode>(value)
            );
        }

        private void SetUpMenus()
        {
            DisplayManager.RegisterScreen(new MainMenuBuilder(DisplayManager)
                .OfName("mainMenu")
                .WithHeader("COVID Management System")
                .AddOption("View Details of a Person", "viewPersonDetails")
                .AddOption("View All Visitors", "viewAllVisitors")
                .AddOption("Safe Entry Management", "safeEntryMenu")
                .AddOption("Travel Entry Management", "travelEntryMenu")
                .AddOption("Explore Global Stats", "globalStats")
                .Build()
            );

            DisplayManager.RegisterScreen(new MenuBuilder(DisplayManager)
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

            DisplayManager.RegisterScreen(new MenuBuilder(DisplayManager)
                .OfName("travelEntryMenu")
                .WithHeader("Travel Entry Management")
                .AddOption("View All SHN Facilities", "viewFacilities")
                .AddOption("New Visitor", "newVisitor")
                .AddOption("New Travel Record", "newTravelRecord")
                .AddOption("Pay SHN Charges", "paySHNCharges")
                .AddOption("Generate SHN Status Report", "shnReport")
                .Build()
            );
        }

        public void Run()
        {
            DisplayManager.Run("mainMenu");
        }
    }
}