﻿using System;
using COVIDMonitoringSystem.Core.PersonMgr;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class ChargeCalculator
    {
        public TravelEntryMatcher Matcher { get; set; }
        public Func<TravelEntry, int> QuarantineDays { get; }
        public Func<TravelEntry, double> TransportCost { get; }
        public Func<TravelEntry, double> SDFCost { get; }


        public ChargeCalculator(TravelEntryMatcher matcher, Func<TravelEntry, int> quarantineDays, Func<TravelEntry, double> transportCost, Func<TravelEntry, double> sdfCost)
        {
            Matcher = matcher;
            QuarantineDays = quarantineDays;
            TransportCost = transportCost;
            SDFCost = sdfCost;
            
            ChargeFactory.RegisterChargeCalculator(this);
        }
    }
}