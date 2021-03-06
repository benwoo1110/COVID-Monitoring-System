//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public class Button : SelectableElement
    {
        public Action Runner { get; set; }
        public ActionMethod MethodRunner { get; set; }

        public Button(string name = null) : base(name)
        {
        }

        public void Run()
        {
            if (MethodRunner != null)
            {
                MethodRunner.Run(TargetScreen);
                return;
            }
            
            Runner?.Invoke();
        }
    }
}