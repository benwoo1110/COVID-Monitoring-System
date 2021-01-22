using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class AccommodationTier
    {
        private static readonly Dictionary<string, AccommodationTier> Types = new Dictionary<string, AccommodationTier>();
        
        public static readonly AccommodationTier None = new AccommodationTier(
            new []{ "New Zealand", "Vietnam" }
        );
        
        public static readonly AccommodationTier OwnAcc = new AccommodationTier(
            new []{ "Macao SAR" }
        );
        
        public static readonly AccommodationTier Dedicated = new AccommodationTier(
            new string[]{ }
        );
        
        private static readonly AccommodationTier FallbackRequirement = Dedicated;

        private static void RegisterRequirement([NotNull] AccommodationTier requirement)
        {
            foreach (var country in requirement.TargetCountries)
            {
                Types.Add(country.ToLower(), requirement);
            }
        }
        
        [NotNull] public static AccommodationTier FindAppropriateTier([NotNull] TravelEntry entry)
        {
            return Types.GetValueOrDefault(entry.LastCountryOfEmbarkation.ToLower()) ?? FallbackRequirement;
        }
        
        public string[] TargetCountries { [NotNull] get; }

        private AccommodationTier(
            [NotNull] string[] targetCountries)
        {
            TargetCountries = targetCountries;
            RegisterRequirement(this);
        }
    }
}