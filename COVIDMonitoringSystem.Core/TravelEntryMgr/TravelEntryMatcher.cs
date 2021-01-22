using System;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class TravelEntryMatcher
    {
        public static TravelEntryMatcher Of(TravelEntry entry)
        {
            return new TravelEntryMatcher(entry.TravelPerson.GetType(), entry.Tier);
        }
        
        public Type PersonType { get; }
        public AccommodationTier Tier { get; }

        public TravelEntryMatcher(Type personType, AccommodationTier tier)
        {
            PersonType = personType;
            Tier = tier;
        }

        public int GenerateIdentifier()
        {
            var code = HashCode.Combine(PersonType, Tier);
            Logging.Debug(code.ToString());
            return code;
        }
    }
}