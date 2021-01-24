//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Logging.SetLogLevel(LogLevel.Debug);
            new ConsoleRunner().Run();
        }
    }
}