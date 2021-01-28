﻿namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public class BuilderScreen : AbstractScreen
    {
        public string ScreenName { get; set; }
        public override string Name => ScreenName;

        public BuilderScreen(ConsoleDisplayManager displayManager) : base(displayManager)
        {
        }
    }
}