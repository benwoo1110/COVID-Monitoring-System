using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Screens
{
    public class LegacyAbstractScreen :  BuilderAbstractScreen
    {
        public Action Runner { get; set; }

        public LegacyAbstractScreen(ConsoleDisplayManager displayManager) : base(displayManager)
        {
        }

        public LegacyAbstractScreen(ConsoleDisplayManager displayManager, string name, string header, Action runner) : base(displayManager) //TODO: Remove header
        {
            ScreenName = name;
            Runner = runner;
        }
        
        public override void OnView()
        {
            ColourSelector.Element();
            Console.SetCursorPosition(0, 4);
            Runner?.Invoke();
            CHelper.WriteEmpty();
            CHelper.WriteLine("Back to menu...");
            DisplayManager.PopScreen();
        }
    }
}