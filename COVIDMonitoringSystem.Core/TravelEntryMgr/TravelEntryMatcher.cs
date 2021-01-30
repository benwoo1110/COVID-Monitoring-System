//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class TravelEntryMatcher
    {
        public static TravelEntryMatcher Of(TravelEntry entry)
        {
            return new TravelEntryMatcher(entry.TravelPerson.GetType(), entry.Tier);
        }
        
        public Type PersonType { get; }
        public SHNTier Tier { get; }

        public TravelEntryMatcher(Type personType, SHNTier tier)
        {
            PersonType = personType;
            Tier = tier;
        }

        public int GenerateIdentifier()
        {
            var code = HashCode.Combine(PersonType, Tier);
            return code;
        }
    }
}