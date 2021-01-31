//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class Values : Attribute
    {
        public string ValueName { get; }

        public Values(string valueName)
        {
            ValueName = valueName;
        }
    }
}