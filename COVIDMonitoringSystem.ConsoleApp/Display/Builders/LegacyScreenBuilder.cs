using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Builders
{
    public class LegacyScreenBuilder : AbstractScreenBuilder<LegacyScreenBuilder, LegacyScreen>
    {
        public LegacyScreenBuilder(ConsoleManager manager) : base(manager)
        {
        }

        public LegacyScreenBuilder WithRunner(Action runner)
        {
            TargetScreen.Runner = runner;
            return this;
        }
    }
}