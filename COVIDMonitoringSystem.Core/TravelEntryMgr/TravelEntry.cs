using System;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.TravelEntryMgr;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core
{
    public class TravelEntry
    {
        public Person TravelPerson { get; set; }
        public string LastCountryOfEmbarkation { get; }
        public string EntryMode { get; }
        public DateTime EntryDate { get; }
        public DateTime ShnEndDate { get; private set; }
        public SHNRequirements RequirementType { get; private set; }
        public SHNFacility ShnFacility { get; private set; }
        public bool IsPaid { get; set; }
        
        public TravelEntry(string lastCountryOfEmbarkation, string entryMode, DateTime entryDate)
        {
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryMode = entryMode;
            EntryDate = entryDate;
            CalculateSHNType();
            CalculateSHNDuration();
        }

        public TravelEntry(string lastCountryOfEmbarkation, string entryMode, DateTime entryDate, DateTime shnEndDate, SHNFacility shnFacility, bool isPaid)
        {
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryMode = entryMode;
            EntryDate = entryDate;
            ShnEndDate = shnEndDate;
            ShnFacility = shnFacility;
            IsPaid = isPaid;
            CalculateSHNType();
        }

        private void CalculateSHNType()
        {
            if (MatchCountry("New Zealand") || MatchCountry("Vietnam"))
            {
                RequirementType = SHNRequirements.None;
                return;
            }

            if (MatchCountry("Macao SAR"))
            {
                RequirementType = SHNRequirements.OwnAcc;
                return;
            }

            RequirementType = SHNRequirements.Dedicated;
        }

        private bool MatchCountry([NotNull] string country)
        {
            return country.ToLower().Equals(LastCountryOfEmbarkation.ToLower());
        }

        private void CalculateSHNDuration()
        {
            ShnEndDate = EntryDate.AddDays((int) RequirementType);
        }

        public void CalculateCharges()
        {
            
        }

        public void AssignSHNFacility(SHNFacility facility)
        {
            if (!RequiresSHNFacility())
            {
                throw new InvalidOperationException($"No dedicated SHN facility needed for type: {RequirementType}");
            }
            
            ShnFacility = facility;
        }

        public bool RequiresSHNFacility()
        {
            return RequirementType != SHNRequirements.Dedicated;
        }
        
        public override string ToString()
        {
            return base.ToString();
        }
    }
}