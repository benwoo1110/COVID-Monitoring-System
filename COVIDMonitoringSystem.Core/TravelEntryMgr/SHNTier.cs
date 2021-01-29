using System.Collections.Generic;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class SHNTier
    {
        private static readonly Dictionary<string, SHNTier> Types = new Dictionary<string, SHNTier>();
        
        public static readonly SHNTier None = new SHNTier(
            new []{ "New Zealand", "Vietnam" }
        );
        
        public static readonly SHNTier OwnAcc = new SHNTier(
            new []{ "Macao SAR" }
        );
        
        public static readonly SHNTier Dedicated = new SHNTier(
            new string[]{ }
        );
        
        private static readonly SHNTier FallbackRequirement = Dedicated;

        private static void RegisterRequirement([NotNull] SHNTier requirement)
        {
            foreach (var country in requirement.TargetCountries)
            {
                Types.Add(country.ToLower(), requirement);
            }
        }

        [NotNull] public static SHNTier FindAppropriateTier(string country)
        {
            return Types.GetValueOrDefault(country.ToLower()) ?? FallbackRequirement;
        }
        
        [NotNull] public static SHNTier FindAppropriateTier([NotNull] TravelEntry entry)
        {
            return FindAppropriateTier(entry.LastCountryOfEmbarkation);
        }

        public string[] TargetCountries { [NotNull] get; }

        private SHNTier(
            [NotNull] string[] targetCountries)
        {
            TargetCountries = targetCountries;
            RegisterRequirement(this);
        }
    }
}