using System;

namespace COVIDMonitoringSystem.Core
{
    public class TravelEntry
    {
        public string LastCountryOfEmbarkation { get; set; }
        public string EntryMode { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ShnEndDate { get; set; }
        public SHNFacility ShnFacility { get; set; }
        public bool IsPaid { get; set; }

        public TravelEntry()
        {
        }

        public TravelEntry(string lastCountryOfEmbarkation, string entryMode, DateTime entryDate)
        {
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryMode = entryMode;
            EntryDate = entryDate;
        }

        public void AssignSHNFacility(SHNFacility facility)
        {
            ShnFacility = facility;
        }

        public void CalculateShnDuration()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}