<<<<<<< HEAD:COVIDMonitoringSystem.ConsoleApp/Display/BuilderScreen.cs
﻿//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

namespace COVIDMonitoringSystem.ConsoleApp.Display
=======
﻿using COVIDMonitoringSystem.ConsoleApp.Display;

namespace COVIDMonitoringSystem.ConsoleApp.Screens
>>>>>>> 366ddc1a39b8c6cb1e1d60ba8cfb7b4cbeba95c5:COVIDMonitoringSystem.ConsoleApp/Screens/BuilderScreen.cs
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