using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.Core;
using COVIDMonitoringSystem.Core.PersonMgr;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public partial class ConsoleRunner
    {
        private Dictionary<string, Menu> MenusCollection { get; }
        private COVIDMonitoringManager Manager { get; }

        public ConsoleRunner()
        {
            Manager = new COVIDMonitoringManager();
            MenusCollection = new Dictionary<string, Menu>();
            SetUpMenus();
        }

        private void SetUpMenus()
        {
            MenusCollection.Add("mainMenu", new MainMenu(
                "COVID Management System",
                new[]
                {
                    new MenuOption("View Details of a Person", ViewPersonDetails),
                    new MenuOption("View All Visitors", ViewAllVisitors),
                    new MenuOption("SafeEntry Management", ManageSafeEntry),
                    new MenuOption("TravelEntry Management", ManageTravelEntry),
                    new MenuOption("Explore Global Stats", ExploreGlobalStats), //TODO: Bonus stuff
                }
            ));

            MenusCollection.Add("travelEntry", new Menu(
                "TravelEntry Management",
                new[]
                {
                    new MenuOption("View All SHN Facilities", ViewAllSHNFacility),
                    new MenuOption("New Visitor", NewVisitor),
                    new MenuOption("New Travel Record", NewTravelRecord),
                    new MenuOption("Pay SHN Charges", PaySHNCharges),
                    new MenuOption("Generate SHN Status Report", GenerateSHNReport),
                }
            ));

            MenusCollection.Add("safeEntry", new Menu(
                "SafeEntry Management",
                new[]
                {
                    new MenuOption("Assign or replace TraceTogether Token", AssignToken),
                    new MenuOption("View all business locations", ViewLocations),
                    new MenuOption("Change capacity of business location", ChangeCapacity),
                    new MenuOption("SafeEntry Check-In", CheckIn),
                    new MenuOption("SafeEntry Check-Out", CheckOut),
                }
            ));
        }

        public void Run()
        {
            MenusCollection["mainMenu"].RunMenuOption();
        }

        private void ViewPersonDetails()
        {
            Console.WriteLine("ViewPersonDetails");
        }

        private void ViewAllVisitors()
        {
            FancyObjectDisplay.PrintList(
                Manager.GetAllPersonOfType<Visitor>(),
                new []{ "Name", "PassportNo", "Nationality", "SafeEntryList", "TravelEntryList" }
            );
        }

        private void ExploreGlobalStats()
        {
            Console.WriteLine("ExploreGlobalStats");
        }
    }
}