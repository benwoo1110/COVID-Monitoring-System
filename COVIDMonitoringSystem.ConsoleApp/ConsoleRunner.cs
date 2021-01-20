using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.Core;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public class ConsoleRunner
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
                    new MenuOption("View Details of a Person's ", ViewPersonDetails),
                    new MenuOption("View All Visitors", ViewAllVisitors),
                    new MenuOption("Assign/Replace TraceTogether Token", ManageTraceTogetherToken),
                    new MenuOption("SafeEntry Management", ManageSafeEntry),
                    new MenuOption("TravelEntry Management", ManageTravelEntry),
                    new MenuOption("Explore Global Stats", Dummy), //TODO: Bonus stuff
                }
            ));

            MenusCollection.Add("travelentry", new Menu(
                "------- TravelEntry Management -------",
                new[]
                {
                    new MenuOption("View SHN Facilities", Dummy),
                    new MenuOption("Create Visitor", Dummy),
                    new MenuOption("Create Travel Record", Dummy),
                    new MenuOption("Calculate SHN Charges", Dummy),
                    new MenuOption("Generate SHN Status Report", Dummy),
                }
            ));
        }

        private void Dummy()
        {
            throw new NotImplementedException();
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

        private void ManageTravelEntry()
        {
            MenusCollection["travelentry"].RunMenuOption();
        }
    }
}