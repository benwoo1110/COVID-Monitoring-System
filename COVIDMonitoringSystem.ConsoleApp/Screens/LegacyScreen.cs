//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Screens
{
    public class LegacyScreen :  BuilderScreen
    {
        public Action Runner { get; set; }

        public LegacyScreen(ConsoleDisplayManager displayManager) : base(displayManager)
        {
        }

        public LegacyScreen(ConsoleDisplayManager displayManager, string name, string header, Action runner) : base(displayManager) //TODO: Remove header
        {
            ScreenName = name;
            Runner = runner;
        }
        
        public override void OnView()
        {
            ColourSelector.Default();
            Console.SetCursorPosition(0, 4);
            Runner?.Invoke();
            CHelper.WriteEmpty();
            CHelper.WriteLine("Back to menu...");
            DisplayManager.PopScreen();
        }
    }
}