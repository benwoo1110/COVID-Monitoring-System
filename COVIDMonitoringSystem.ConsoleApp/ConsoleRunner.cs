using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.Core;
using Cryptography.ConsoleApp;

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
            MenusCollection.Add("mainmenu", new Menu(
                "------- COVID Management System -------",
                new[]
                {
                    new MenuOption("View Details of a Person's ", ViewPersonDetails),
                    new MenuOption("View All Visitors", ViewAllVisitors),
                    new MenuOption("Assign/Replace TraceTogether Token", ManageTraceTogetherToken),
                    new MenuOption("SafeEntry Management", ManageSafeEntry),
                    new MenuOption("TravelEntry Management", ManageTravelEntry),
                    new MenuOption("Explore Global Stats", null), //TODO: Bonus stuff
                }
            ));
        }

        public void Run()
        {
            MenusCollection["mainmenu"].RunMenuOptionLooped();
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
            Console.WriteLine("ManageTravelEntry");
        }
    }
}