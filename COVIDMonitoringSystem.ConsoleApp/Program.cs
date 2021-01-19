//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using COVIDMonitoringSystem.Core;
using Cryptography.ConsoleApp;

namespace COVIDMonitoringSystem.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var covidMonitoringManager = new COVIDMonitoringManager();

            Menu mainMenu = new Menu(
                "====== Main ======",
                new[]
                {
                    "General",
                    "Business",
                    "Save Entry",
                    "SHN"
                }
            );

            mainMenu.RunMenuOption();
        }

        public void GeneralActions()
        {
            
        }
        
        public void TraceTogetherActions()
        {
            
        }
        
        public void SafeEntryActions()
        {
            
        }
        
        public void TravelEntryActions()
        {
            
        }
    }
}