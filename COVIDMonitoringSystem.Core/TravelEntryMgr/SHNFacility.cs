using System;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class SHNFacility
    {
        public string FacilityName { get; }
        public int FacilityCapacity { get; }
        public int FacilityVacancy { get; }
        public CheckPointDistance Distance { get; }

        public SHNFacility(string facilityName, int facilityCapacity, double distFromAirCheckpoint, double distFromSeaCheckpoint, double distFromLandCheckpoint)
        {
            FacilityName = facilityName;
            FacilityCapacity = facilityCapacity;
            FacilityVacancy = facilityCapacity;
            Distance = new CheckPointDistance(distFromAirCheckpoint, distFromSeaCheckpoint, distFromLandCheckpoint);
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