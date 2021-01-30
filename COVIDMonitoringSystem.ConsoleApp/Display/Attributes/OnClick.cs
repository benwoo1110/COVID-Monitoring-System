//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class OnClick : Attribute
    {
        public string ButtonName { get; }
        
        public OnClick(string buttonName)
        {
            ButtonName = buttonName;
        }
    }
}