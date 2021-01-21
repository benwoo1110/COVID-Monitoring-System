using System;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class TravelEntryMatcher
    {
        public Type ClassType { get; }
        public SHNType Type { get; }

        public TravelEntryMatcher(Type classType, SHNType type)
        {
            ClassType = classType;
            Type = type;
        }

        public int GenerateIdentifier()
        {
            var code = HashCode.Combine(ClassType, Type);
            Logging.Debug(code.ToString());
            return code;
        }
    }
}