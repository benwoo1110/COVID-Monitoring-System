//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using COVIDMonitoringSystem.ConsoleApp.Builders;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Screens.General;
using COVIDMonitoringSystem.ConsoleApp.Screens.SafeEntryMgr;
using COVIDMonitoringSystem.ConsoleApp.Screens.TravelEntryMgr;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.SafeEntryMgr;
using COVIDMonitoringSystem.Core.TravelEntryMgr;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public class ConsoleRunner
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
            DisplayManager.ResolveManager.RegisterQuickObjectResolver(
                Manager.FindPerson,
                "No person with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterQuickObjectResolver(
                Manager.FindPersonOfType<Resident>,
                "No resident with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterQuickObjectResolver(
                Manager.FindPersonOfType<Visitor>,
                "No visitor with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterQuickObjectResolver(
                Manager.FindBusinessLocation,
                "No business with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterQuickObjectResolver(
                Manager.FindSHNFacility,
                "No SHN facility with name {0} found."
            );
            DisplayManager.ResolveManager.RegisterQuickObjectResolver(
                SHNTier.FindAppropriateTier,
                "This shouldn't happen. Input was: {0}"
            );
            DisplayManager.ResolveManager.RegisterQuickEnumResolver<TravelEntryMode>("entry mode");
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
                .AddOption("Pay SHN Charges", "viewSHNCharges")
                .AddOption("Generate SHN Status Report", "shnReport")
                .Build()
            );
        }

        private void SetUpGeneralScreens()
        {
            DisplayManager.RegisterScreen(new PersonDetailsScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new ListScreenBuilder<Visitor>(DisplayManager)
                .OfName("viewAllVisitors")
                .WithHeader("View All Visitors")
                .WithProperties(new[] {"Name", "PassportNo", "Nationality", "SafeEntryList", "TravelEntryList"})
                .WithGetter(() => Manager.GetAllPersonOfType<Visitor>())
                .Build()
            );
            DisplayManager.RegisterScreen(new GlobalStatsScreen(DisplayManager, Manager));
        }

        private void SetUpSafeEntryScreens()
        {
            DisplayManager.RegisterScreen(new TokenScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new ListScreenBuilder<BusinessLocation>(DisplayManager)
                .OfName("viewLocations")
                .WithHeader("View All Business Location")
                .WithProperties(new[] {"BusinessName", "BranchCode", "MaximumCapacity", "VisitorsNow"})
                .WithGetter(() => Manager.BusinessLocationList)
                .Build()
            );
            DisplayManager.RegisterScreen(new EditCapacityScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new CheckInScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new CheckOutScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new ContactTraceScreen(DisplayManager, Manager));
        }

        private void SetUpTravelEntryScreens()
        {
            DisplayManager.RegisterScreen(new ListScreenBuilder<SHNFacility>(DisplayManager)
                .OfName("viewFacilities")
                .WithHeader("View All SHN Facilities")
                .WithProperties(new[]
                    {"FacilityName", "FacilityCapacity", "FacilityVacancy", "FromLand", "FromSea", "FromAir"})
                .WithGetter(() => Manager.SHNFacilitiesList)
                .Build()
            );
            DisplayManager.RegisterScreen(new NewVisitorScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new NewTravelRecordScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new ViewSHNChargesScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new PaySHNChargesScreen(DisplayManager, Manager));
            DisplayManager.RegisterScreen(new GenerateSHNReportScreen(DisplayManager, Manager));
        }

        public void Run()
        {
            DisplayManager.Run("mainMenu");
        }
    }
}