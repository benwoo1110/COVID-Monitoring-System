using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Screens
{
    public class LegacyScreen : Screen
    {
        public Action Runner { get; set; }

        public LegacyScreen(ConsoleDisplayManager displayDisplayManager) : base(displayDisplayManager)
        {
        }

        public LegacyScreen(ConsoleDisplayManager displayDisplayManager, string name, string header, Action runner) : base(displayDisplayManager) //TODO: Remove header
        {
            Name = name;
            Runner = runner;
        }

        public override void OnView()
        {
            ColourSelector.Element();
            Console.SetCursorPosition(0, 4);
            Runner?.Invoke();
            CHelper.WriteEmpty();
            CHelper.WriteLine("Back to menu...");
            DisplayDisplayManager.PopScreen();
        }
    }
}