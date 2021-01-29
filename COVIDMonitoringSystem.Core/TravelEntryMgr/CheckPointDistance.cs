//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class CheckPointDistance
    {
        public double FromLand { get; }
        public double FromSea { get; }
        public double FromAir { get; }

        public CheckPointDistance(double fromLand, double fromSea, double fromAir)
        {
            FromLand = fromLand;
            FromSea = fromSea;
            FromAir = fromAir;
        }

        public double FromMode(TravelEntryMode mode)
        {
            return mode switch
            {
                TravelEntryMode.Land => FromLand,
                TravelEntryMode.Sea => FromSea,
                TravelEntryMode.Air => FromAir,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}