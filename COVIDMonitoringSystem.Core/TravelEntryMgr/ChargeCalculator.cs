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
            (tr) => 20,
            (tr) => 0
        );
        
        private static readonly ChargeCalculator ResidentDedicated = new ChargeCalculator(
            new TravelEntryType(typeof(Resident), SHNRequirement.Dedicated),
            (tr) => 20,
            (tr) => 1000
        );
        
        private static readonly ChargeCalculator VisitorNone = new ChargeCalculator(
            new TravelEntryType(typeof(Visitor), SHNRequirement.None), 
            (tr) => 80,
            (tr) => 0
        );

        private static readonly ChargeCalculator VisitorOwnAcc = new ChargeCalculator(
            new TravelEntryType(typeof(Visitor), SHNRequirement.OwnAcc),
            (tr) => 80,
            (tr) => 0
        );
        
        private static readonly ChargeCalculator VisitorDedicated = new ChargeCalculator(
            new TravelEntryType(typeof(Visitor), SHNRequirement.Dedicated),
            (tr) =>
            {
                var fare = 50 + tr.ShnFacility.CalculateTravelCost(tr);
                // TODO: Surcharge

                return fare;
            },
            (tr) => 2000
        );

        private static void RegisterChargeCalculator([NotNull] ChargeCalculator calculator)
        {
            Charges.Add(calculator.EntryType.GenerateIdentifier(), calculator);
        }

        [CanBeNull] public static ChargeCalculator FindAppropriateCalculator([NotNull] TravelEntry entry)
        {
            var matcher = new TravelEntryType(
                entry.TravelPerson.GetType(),
                SHNRequirement.FindAppropriateType(entry)
            );
            return Charges.GetValueOrDefault(matcher.GenerateIdentifier());
        }
        
        public TravelEntryType EntryType { [NotNull] get; }
        public Func<TravelEntry, double> TransportCost { [NotNull] get; }
        public Func<TravelEntry, double> SDFCost { [NotNull] get; }

        private ChargeCalculator(
            [NotNull] TravelEntryType type, 
            [NotNull] Func<TravelEntry, double> transportCost, 
            [NotNull] Func<TravelEntry, double> sdfCost)
        {
            EntryType = type;
            TransportCost = transportCost;
            SDFCost = sdfCost;
            RegisterChargeCalculator(this);
        }
    }
}