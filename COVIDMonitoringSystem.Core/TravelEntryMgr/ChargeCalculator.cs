using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.Core.PersonMgr;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class ChargeCalculator
    {
        public const double SwapTestCost = 200;

        private static readonly Dictionary<int, ChargeCalculator> Charges = new Dictionary<int, ChargeCalculator>();

        private static readonly ChargeCalculator ResidentNone = new ChargeCalculator(
            new TravelEntryType(typeof(Resident), SHNRequirement.None), 
            (tr) => 0,
            (tr) => 0
        );

        private static readonly ChargeCalculator ResidentOwnAcc = new ChargeCalculator(
            new TravelEntryType(typeof(Resident), SHNRequirement.OwnAcc),
            (tr) => 0,
            (tr) => 0
        );
        
        private static readonly ChargeCalculator ResidentDedicated = new ChargeCalculator(
            new TravelEntryType(typeof(Resident), SHNRequirement.Dedicated),
            (tr) => 0,
            (tr) => 0
        );
        
        private static readonly ChargeCalculator VisitorNone = new ChargeCalculator(
            new TravelEntryType(typeof(Visitor), SHNRequirement.None), 
            (tr) => 0,
            (tr) => 0
        );

        private static readonly ChargeCalculator VisitorOwnAcc = new ChargeCalculator(
            new TravelEntryType(typeof(Visitor), SHNRequirement.OwnAcc),
            (tr) => 0,
            (tr) => 0
        );
        
        private static readonly ChargeCalculator VisitorDedicated = new ChargeCalculator(
            new TravelEntryType(typeof(Visitor), SHNRequirement.Dedicated),
            (tr) => 0,
            (tr) => 0
        );

        public static void RegisterChargeCalculator(ChargeCalculator calculator)
        {
            Charges.Add(calculator.EntryType.GenerateIdentifier(), calculator);
        }

        public static ChargeCalculator FindAppropriateCalculator(TravelEntry entry)
        {
            var shnType = SHNRequirement.FindAppropriateType(entry);
            var personType = entry.TravelPerson.GetType();
            var matcher = new TravelEntryType(personType, shnType);

            return Charges.GetValueOrDefault(matcher.GenerateIdentifier());
        }
        
        public TravelEntryType EntryType { get; }
        public Func<TravelEntry, double> TransportCost { get; }
        public Func<TravelEntry, double> SDFCost { get; }
        
        public ChargeCalculator(TravelEntryType type, Func<TravelEntry, double> transportCost, Func<TravelEntry, double> sdfCost)
        {
            EntryType = type;
            TransportCost = transportCost;
            SDFCost = sdfCost;
            
            RegisterChargeCalculator(this);
        }
    }
}