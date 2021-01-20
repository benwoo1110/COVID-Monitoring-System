using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.Core;

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
            MenusCollection.Add("mainmenu", new MainMenu(
                "------- COVID Management System -------",
                new[]
                {
                    new MenuOption("View Details of a Person", ViewPersonDetails),
                    new MenuOption("View All Visitors", ViewAllVisitors),
                    new MenuOption("Assign/Replace TraceTogether Token", ManageTraceTogetherToken),
                    new MenuOption("SafeEntry Management", ManageSafeEntry),
                    new MenuOption("TravelEntry Management", ManageTravelEntry),
                    new MenuOption("Explore Global Stats", ExploreGlobalStats), //TODO: Bonus stuff
                }
            ));

            MenusCollection.Add("travelentry", new Menu(
                "------- TravelEntry Management -------",
                new[]
                {
                    new MenuOption("View All SHN Facilities", ViewAllSHNFacility),
                    new MenuOption("New Visitor", NewVisitor),
                    new MenuOption("New Travel Record", NewTravelRecord),
                    new MenuOption("Pay SHN Charges", PaySHNCharges),
                    new MenuOption("Generate SHN Status Report", GenerateSHNReport),
                }
            ));
        }

        public void Run()
        {
            MenusCollection["mainmenu"].RunMenuOption();
        }

        private void ViewPersonDetails()
        {
            Console.WriteLine("ViewPersonDetails");
        }

        private void ViewAllVisitors()
        {
            Console.WriteLine("ViewAllVisitors");
        }

        private void ManageTraceTogetherToken()
        {
            Console.WriteLine("ManageTraceTogetherToken");
        }

        private void ManageSafeEntry()
        {
            Console.WriteLine("ManageSafeEntry");
        }

        private void ExploreGlobalStats()
        {
            Console.WriteLine("ExploreGlobalStats");
        }
    }
}