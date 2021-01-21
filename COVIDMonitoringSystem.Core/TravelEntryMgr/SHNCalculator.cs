using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.Core.PersonMgr;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class SHNCalculator
    {
        public const double SwapTestCost = 200;

        private static readonly Dictionary<int, SHNCalculator> Charges = new Dictionary<int, SHNCalculator>();

        public static readonly SHNCalculator ResidentNone = new SHNCalculator(
            new TravelEntryMatcher(typeof(Resident), SHNRequirement.None), 
            (tr) => 0,
            (tr) => 0
        );

        public static readonly SHNCalculator ResidentOwnAcc = new SHNCalculator(
            new TravelEntryMatcher(typeof(Resident), SHNRequirement.OwnAcc),
            (tr) => 20,
            (tr) => 0
        );

        public static readonly SHNCalculator ResidentDedicated = new SHNCalculator(
            new TravelEntryMatcher(typeof(Resident), SHNRequirement.Dedicated),
            (tr) => 20,
            (tr) => 1000
        );

        public static readonly SHNCalculator VisitorNone = new SHNCalculator(
            new TravelEntryMatcher(typeof(Visitor), SHNRequirement.None), 
            (tr) => 80,
            (tr) => 0
        );

        public static readonly SHNCalculator VisitorOwnAcc = new SHNCalculator(
            new TravelEntryMatcher(typeof(Visitor), SHNRequirement.OwnAcc),
            (tr) => 80,
            (tr) => 0
        );

        public static readonly SHNCalculator VisitorDedicated = new SHNCalculator(
            new TravelEntryMatcher(typeof(Visitor), SHNRequirement.Dedicated),
            (tr) =>
            {
                var fare = 50 + tr.ShnFacility.CalculateTravelCost(tr);
                // TODO: Surcharge

                return fare;
            },
            (tr) => 2000
        );

        private static void RegisterChargeCalculator([NotNull] TravelEntryMatcher matcher, [NotNull] SHNCalculator calculator)
        {
            Charges.Add(matcher.GenerateIdentifier(), calculator);
        }

        [CanBeNull] public static SHNCalculator FindAppropriateCalculator([NotNull] TravelEntry entry)
        {
            return Charges.GetValueOrDefault(TravelEntryMatcher.Of(entry).GenerateIdentifier());
        }
        
        public Func<TravelEntry, double> TransportCost { [NotNull] get; }
        public Func<TravelEntry, double> SDFCost { [NotNull] get; }

        private SHNCalculator(
            [NotNull] TravelEntryMatcher matcher,
            [NotNull] Func<TravelEntry, double> transportCost, 
            [NotNull] Func<TravelEntry, double> sdfCost)
        {
            TransportCost = transportCost;
            SDFCost = sdfCost;
            RegisterChargeCalculator(matcher, this);
        }
    }
}