using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Screens;

namespace COVIDMonitoringSystem.ConsoleApp.Builders
{
    public class LegacyScreenBuilder : AbstractScreenBuilder<LegacyScreenBuilder, LegacyScreen>
    {
        public LegacyScreenBuilder(ConsoleDisplayManager displayManager) : base(displayManager)
        {
        }

        public LegacyScreenBuilder WithRunner(Action runner)
        {
            TargetScreen.Runner = runner;
            return this;
        }
    }
}