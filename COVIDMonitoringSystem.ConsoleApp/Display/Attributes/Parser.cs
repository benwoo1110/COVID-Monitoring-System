//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class Parser : Attribute
    {
        public string InputName { get; }
        public string TargetLabel { get; }

        public Parser(string inputName, string targetLabel = null)
        {
            InputName = inputName;
            TargetLabel = targetLabel;
        }
    }
}