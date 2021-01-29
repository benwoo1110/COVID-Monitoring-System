//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;

namespace COVIDMonitoringSystem.Core.Utilities
{
    public static class Extensions
    {
        public static bool BetweenTimeOf(this DateTime dateTime, string from, string to)
        {
            var fromTime = TimeSpan.Parse(from);
            var toTime = TimeSpan.Parse(to);
            var currentTime = dateTime.TimeOfDay;
            return currentTime >= fromTime && currentTime < toTime;
        }
    }
}