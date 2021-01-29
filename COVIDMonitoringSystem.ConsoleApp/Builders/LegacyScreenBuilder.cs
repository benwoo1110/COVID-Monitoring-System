//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

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