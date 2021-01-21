using System.Collections.Generic;
using System.Linq;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class SHNRequirement
    {
        private static readonly Dictionary<string, SHNRequirement> Types = new Dictionary<string, SHNRequirement>();
        
        public static readonly SHNRequirement None = new SHNRequirement(
            0, 
            new []{ "New Zealand", "Vietnam" }
        );
        
        public static readonly SHNRequirement OwnAcc = new SHNRequirement(
            0, 
            new []{ "Macao SAR" }
        );
        
        public static readonly SHNRequirement Dedicated = new SHNRequirement(
            0, 
            new string[]{ }
        );
        
        private static readonly SHNRequirement FallbackRequirement = Dedicated;

        public static void RegisterSHNType(SHNRequirement requirement)
        {
            foreach (var country in requirement.TargetCountries)
            {
                Types.Add(country, requirement);
            }
        }
        
        public static SHNRequirement FindAppropriateType(TravelEntry entry)
        {
            return Types.GetValueOrDefault(entry.LastCountryOfEmbarkation) ?? FallbackRequirement;
        }

        public int QuarantineDays { get; }
        public string[] TargetCountries { get; }

        public SHNRequirement(int quarantineDays, string[] targetCountries)
        {
            QuarantineDays = quarantineDays;
            TargetCountries = targetCountries;
            
            RegisterSHNType(this);
        }

        public bool IsThisType(string country)
        {
            return TargetCountries.Contains(country);
        }
    }
}