﻿using System.Collections.Generic;
using COVIDMonitoringSystem.Core.PersonMgr;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public static class ChargeFactory
    {
        public const double SwapTestCost = 200;
        
        private static readonly ChargeCalculator ResidentsNone = new ChargeCalculator(
            new ChargeMatcher(typeof(Resident), SHNType.None), 
            (tr) => 0,
            (tr) => 0,
            (tr) => 0
        );

        private static readonly ChargeCalculator ResidentsOwnAcc = new ChargeCalculator(
            new ChargeMatcher(typeof(Resident), SHNType.OwnAcc),
            (tr) => 7,
            (tr) => 0,
            (tr) => 0
        );

        private static readonly Dictionary<int, ChargeCalculator> Charges = new Dictionary<int, ChargeCalculator>()
        {
            {ResidentsNone.Matcher.GenerateIdentifier(), ResidentsNone},
            {ResidentsOwnAcc.Matcher.GenerateIdentifier(), ResidentsOwnAcc}
        };

        public static ChargeCalculator FindAppropriateCalculator(TravelEntry entry)
        {
            var shnType = CalculateSHNType(entry);
            var personType = entry.TravelPerson.GetType();
            var chargeMatcher = new ChargeMatcher(personType, shnType);

            return Charges.GetValueOrDefault(chargeMatcher.GenerateIdentifier());
        }

        private static SHNType CalculateSHNType([NotNull] TravelEntry entry)
        {
            if (MatchCountry(entry, "New Zealand") || MatchCountry(entry, "Vietnam"))
            {
                return SHNType.None;
            }
            if (MatchCountry(entry, "Macao SAR"))
            {
                return SHNType.OwnAcc;
            }
            return SHNType.Dedicated;
        }

        private static bool MatchCountry([NotNull] TravelEntry entry, [NotNull] string country)
        {
            return country.ToLower().Equals(entry.LastCountryOfEmbarkation.ToLower());
        }
    }
}