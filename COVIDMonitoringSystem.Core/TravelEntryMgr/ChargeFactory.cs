using System.Collections.Generic;
using COVIDMonitoringSystem.Core.PersonMgr;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public static class ChargeFactory
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

        private static bool MatchCountry([NotNull] TravelEntry entry, [NotNull] string country)
        {
            return country.ToLower().Equals(entry.LastCountryOfEmbarkation.ToLower());
        }
    }
}