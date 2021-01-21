using System;
using COVIDMonitoringSystem.Core.TravelEntryMgr;

namespace COVIDMonitoringSystem.Core
{
    public class SHNFacility
    {
        public string FacilityName { get; set; }
        public int FacilityCapacity { get; set; }
        public int FacilityVacancy { get; set; }
        public double DistFromAirCheckpoint { get; set; }
        public double DistFromSeaCheckpoint { get; set; }
        public double DistFromLandCheckpoint { get; set; }

        public SHNFacility(string facilityName, int facilityCapacity, double distFromAirCheckpoint, double distFromSeaCheckpoint, double distFromLandCheckpoint)
        {
            FacilityName = facilityName;
            FacilityCapacity = facilityCapacity;
            FacilityVacancy = facilityCapacity;
            DistFromAirCheckpoint = distFromAirCheckpoint;
            DistFromSeaCheckpoint = distFromSeaCheckpoint;
            DistFromLandCheckpoint = distFromLandCheckpoint;
        }

        public double CalculateTravelCost(TravelEntry entry)
        {
            double dist;
            
            switch (entry.EntryMode)
            {
                case TravelEntryMode.Land:
                    dist = DistFromLandCheckpoint;
                    break;
                case TravelEntryMode.Sea:
                    dist = DistFromSeaCheckpoint;
                    break;
                case TravelEntryMode.Air:
                    dist = DistFromAirCheckpoint;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return dist * 0.22;
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