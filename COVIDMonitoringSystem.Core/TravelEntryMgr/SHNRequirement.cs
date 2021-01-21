using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class SHNRequirement
    {
        private static readonly Dictionary<string, SHNRequirement> Types = new Dictionary<string, SHNRequirement>();
        
        public static readonly SHNRequirement None = new SHNRequirement(
            0, 
            true,
            false,
            new []{ "New Zealand", "Vietnam" }
        );
        
        public static readonly SHNRequirement OwnAcc = new SHNRequirement(
            7,
            true,
            false,
            new []{ "Macao SAR" }
        );
        
        public static readonly SHNRequirement Dedicated = new SHNRequirement(
            14,
            true,
            true,
            new string[]{ }
        );
        
        private static readonly SHNRequirement FallbackRequirement = Dedicated;

        private static void RegisterRequirement([NotNull] SHNRequirement requirement)
        {
            foreach (var country in requirement.TargetCountries)
            {
                Types.Add(country.ToLower(), requirement);
            }
        }
        
        [NotNull] public static SHNRequirement FindAppropriateRequirement([NotNull] TravelEntry entry)
        {
            return Types.GetValueOrDefault(entry.LastCountryOfEmbarkation.ToLower()) ?? FallbackRequirement;
        }

        public int QuarantineDays { [NotNull] get; }
        public bool NeedSwapTest { [NotNull] get; }
        public bool RequiresSHNFacility { [NotNull] get; }
        public string[] TargetCountries { [NotNull] get; }

        private SHNRequirement(
            int quarantineDays,
            bool needSwapTest,
            bool requiresShnFacility,
            [NotNull] string[] targetCountries)
        {
            QuarantineDays = quarantineDays;
            TargetCountries = targetCountries;
            NeedSwapTest = needSwapTest;
            RequiresSHNFacility = requiresShnFacility;
            RegisterRequirement(this);
        }
    }
}