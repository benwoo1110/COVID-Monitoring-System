using System;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class TravelEntryMatcher
    {
        public static TravelEntryMatcher Of(TravelEntry entry)
        {
            return new TravelEntryMatcher(entry.TravelPerson.GetType(), entry.Requirement);
        }
        
        public Type PersonType { get; }
        public SHNRequirement RequirementType { get; }

        public TravelEntryMatcher(Type personType, SHNRequirement requirementType)
        {
            PersonType = personType;
            RequirementType = requirementType;
        }

        public int GenerateIdentifier()
        {
            var code = HashCode.Combine(PersonType, RequirementType);
            Logging.Debug(code.ToString());
            return code;
        }
    }
}