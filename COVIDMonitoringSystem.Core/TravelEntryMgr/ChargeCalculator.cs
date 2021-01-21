using System;
using COVIDMonitoringSystem.Core.PersonMgr;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class ChargeCalculator
    {
        public TravelEntryType EntryType { get; }
        public Func<TravelEntry, double> TransportCost { get; }
        public Func<TravelEntry, double> SDFCost { get; }
        
        public ChargeCalculator(TravelEntryType type, Func<TravelEntry, double> transportCost, Func<TravelEntry, double> sdfCost)
        {
            EntryType = type;
            TransportCost = transportCost;
            SDFCost = sdfCost;
            
            ChargeFactory.RegisterChargeCalculator(this);
        }
    }
}