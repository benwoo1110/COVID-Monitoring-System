using System;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class TravelEntryType
    {
        public Type PersonType { get; }
        public SHNRequirement RequirementType { get; }

        public TravelEntryType(Type personType, SHNRequirement requirementType)
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