using System;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class SHNFacility
    {
        public string FacilityName { get; }
        public int FacilityCapacity { get; }
        public int FacilityVacancy { get; }
        public CheckPointDistance Distance { get; }

        [Obsolete("Required by assignment")]
        public double DistFromLandCheckpoint => Distance.FromLand;
        [Obsolete("Required by assignment")]
        public double DistFromSeaCheckpoint => Distance.FromSea;
        [Obsolete("Required by assignment")]
        public double DistFromAirCheckpoint => Distance.FromAir;

        public SHNFacility(string facilityName, int facilityCapacity, double distFromAirCheckpoint, double distFromSeaCheckpoint, double distFromLandCheckpoint)
        {
            FacilityName = facilityName;
            FacilityCapacity = facilityCapacity;
            FacilityVacancy = facilityCapacity;
            Distance = new CheckPointDistance(distFromAirCheckpoint, distFromSeaCheckpoint, distFromLandCheckpoint);
        }

        public double CalculateTravelCost(TravelEntryMode entryMode, DateTime entryDate)
        {
            var price = 50 + Distance.FromMode(entryMode) * 0.22;
            if (entryDate.BetweenTimeOf("6:00", "9:00") || entryDate.BetweenTimeOf("18:00", "00:00"))
            {
                price *= 1.25;
            }
            else if (entryDate.BetweenTimeOf("00:00", "6:00"))
            {
                price *= 1.50;
            }

            return price;
            
        }

        public double CalculateTravelCost(TravelEntry entry)
        {
            //TODO Check ShnStay matching
            return CalculateTravelCost(entry.EntryModeType, entry.EntryDate);
        }

        [Obsolete("Required by assignment.")]
        public double CalculateTravelCost(string entryMode, DateTime entryDate)
        {
            var entryModeEnum = CoreHelper.ParseEnum<TravelEntryMode>(entryMode);
            return CalculateTravelCost(entryModeEnum, entryDate);
        }

        public bool IsAvailable()
        {
            return FacilityVacancy > 0;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}