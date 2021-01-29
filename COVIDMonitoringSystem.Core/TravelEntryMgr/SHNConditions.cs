//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System.Collections.Generic;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.TravelEntryMgr;
using JetBrains.Annotations;

public delegate double CostCalculator(TravelEntry tr);

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class SHNConditions
    {
        private static readonly CostCalculator FreeCalculator = tr => 0;
        private static readonly Dictionary<int, SHNConditions> Charges = new Dictionary<int, SHNConditions>();

        public static readonly SHNConditions ResidentNone = new SHNConditionsBuilder()
            .WithSwapTest()
            .WithMatcher(new TravelEntryMatcher(typeof(Resident), SHNTier.None))
            .Build();

        public static readonly SHNConditions ResidentOwnAcc = new SHNConditionsBuilder()
            .WithQuarantineDays(7)
            .WithSwapTest()
            .WithTransport(tr => 20)
            .WithMatcher(new TravelEntryMatcher(typeof(Resident), SHNTier.OwnAcc))
            .Build();

        public static readonly SHNConditions ResidentDedicated = new SHNConditionsBuilder()
            .WithQuarantineDays(14)
            .WithSwapTest()
            .WithTransport(tr => 20)
            .WithDedicatedFacility(tr => 1000)
            .WithMatcher(new TravelEntryMatcher(typeof(Resident), SHNTier.Dedicated))
            .Build();

        public static readonly SHNConditions VisitorNone = new SHNConditionsBuilder()
            .WithQuarantineDays(0)
            .WithSwapTest()
            .WithTransport(tr => 80)
            .WithMatcher(new TravelEntryMatcher(typeof(Visitor), SHNTier.None))
            .Build();

        public static readonly SHNConditions VisitorOwnAcc = new SHNConditionsBuilder()
            .WithQuarantineDays(7)
            .WithSwapTest()
            .WithTransport(tr => 80)
            .WithMatcher(new TravelEntryMatcher(typeof(Visitor), SHNTier.OwnAcc))
            .Build();

        public static readonly SHNConditions VisitorDedicated = new SHNConditionsBuilder()
            .WithQuarantineDays(14)
            .WithSwapTest()
            .WithTransport(tr => tr.ShnFacility.CalculateTravelCost(tr))
            .WithDedicatedFacility(tr => 2000)
            .WithMatcher(new TravelEntryMatcher(typeof(Visitor), SHNTier.Dedicated))
            .Build();

        public static void RegisterChargeCalculator([NotNull] TravelEntryMatcher matcher, [NotNull] SHNConditions conditions)
        {
            Charges.Add(matcher.GenerateIdentifier(), conditions);
        }

        [CanBeNull] public static SHNConditions FindAppropriateCalculator([NotNull] TravelEntry entry)
        {
            return Charges.GetValueOrDefault(TravelEntryMatcher.Of(entry).GenerateIdentifier());
        }

        public int QuarantineDays { [NotNull] get; [NotNull] internal set; }
        public bool RequireSwapTest { [NotNull] get; [NotNull] internal set; }
        public bool RequireTransport { [NotNull] get; [NotNull] internal set; }
        public bool RequireDedicatedFacility { [NotNull] get; [NotNull] internal set; }
        public CostCalculator SwapTestCost { [NotNull] get; [NotNull] internal set; } = FreeCalculator;
        public CostCalculator TransportCost { [NotNull] get; [NotNull] internal set; } = FreeCalculator;
        public CostCalculator DedicatedFacilityCost { [NotNull] get; [NotNull] internal set; } = FreeCalculator;

        internal SHNConditions()
        {
        }

        public double CalculateCharges(TravelEntry tr)
        {
            return SwapTestCost(tr) + TransportCost(tr) + DedicatedFacilityCost(tr);
        }
    }
}