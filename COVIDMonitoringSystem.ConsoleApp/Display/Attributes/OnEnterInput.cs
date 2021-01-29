//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class OnEnterInput : Attribute
    {
        public string InputName { get; }

        public OnEnterInput(string inputName)
        {
            InputName = inputName;
        }
    }
}