using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.Core.PersonMgr;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class SHNConditions
    {
        private static readonly Func<TravelEntry, double> FreeCalculator = tr => 0;
        private static readonly Dictionary<int, SHNConditions> Charges = new Dictionary<int, SHNConditions>();

        public static readonly SHNConditions ResidentNone = new SHNConditionsBuilder()
            .WithQuarantineDays(0)
            .WithSwapTest()
            .WithMatcher(new TravelEntryMatcher(typeof(Resident), AccommodationTier.None))
            .Build();

        public static readonly SHNConditions ResidentOwnAcc = new SHNConditionsBuilder()
            .WithQuarantineDays(7)
            .WithSwapTest()
            .WithTransport(tr => 20)
            .WithMatcher(new TravelEntryMatcher(typeof(Resident), AccommodationTier.OwnAcc))
            .Build();

        public static readonly SHNConditions ResidentDedicated = new SHNConditionsBuilder()
            .WithQuarantineDays(14)
            .WithSwapTest()
            .WithTransport(tr => 20)
            .WithDedicatedFacility(tr => 1000)
            .WithMatcher(new TravelEntryMatcher(typeof(Resident), AccommodationTier.Dedicated))
            .Build();

        public static readonly SHNConditions VisitorNone = new SHNConditionsBuilder()
            .WithQuarantineDays(0)
            .WithSwapTest()
            .WithTransport(tr => 80)
            .WithMatcher(new TravelEntryMatcher(typeof(Visitor), AccommodationTier.None))
            .Build();

        public static readonly SHNConditions VisitorOwnAcc = new SHNConditionsBuilder()
            .WithQuarantineDays(7)
            .WithSwapTest()
            .WithTransport(tr => 80)
            .WithMatcher(new TravelEntryMatcher(typeof(Visitor), AccommodationTier.OwnAcc))
            .Build();

        public static readonly SHNConditions VisitorDedicated = new SHNConditionsBuilder()
            .WithQuarantineDays(14)
            .WithSwapTest()
            .WithTransport(tr => 100) //TODO: Do real calculation
            .WithDedicatedFacility(tr => 2000)
            .WithMatcher(new TravelEntryMatcher(typeof(Visitor), AccommodationTier.Dedicated))
            .Build();

        public static void RegisterChargeCalculator([NotNull] TravelEntryMatcher matcher, [NotNull] SHNConditions conditions)
        {
            Charges.Add(matcher.GenerateIdentifier(), conditions);
        }

        [CanBeNull] public static SHNConditions FindAppropriateCalculator([NotNull] TravelEntry entry)
        {
            return Charges.GetValueOrDefault(TravelEntryMatcher.Of(entry).GenerateIdentifier());
        }

        public int QuarantineDays { [NotNull] get; [NotNull] internal set; } = -1;
        public bool RequireSwapTest { [NotNull] get; [NotNull] internal set; }
        public bool RequireTransport { [NotNull] get; [NotNull] internal set; }
        public bool RequireDedicatedFacility { [NotNull] get; [NotNull] internal set; }
        public Func<TravelEntry, double> SwapTestCost { [NotNull] get; [NotNull] internal set; } = FreeCalculator;
        public Func<TravelEntry, double> TransportCost { [NotNull] get; [NotNull] internal set; } = FreeCalculator;
        public Func<TravelEntry, double> DedicatedFacilityCost { [NotNull] get; [NotNull] internal set; } = FreeCalculator;

        internal SHNConditions()
        {
        }

        public double CalculateCharges(TravelEntry tr)
        {
            return SwapTestCost(tr) + TransportCost(tr) + DedicatedFacilityCost(tr);
        }
    }
}