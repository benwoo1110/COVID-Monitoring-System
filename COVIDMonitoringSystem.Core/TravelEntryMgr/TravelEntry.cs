using System;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.Utilities;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class TravelEntry
    {
        public Person TravelPerson { get; }
        public string LastCountryOfEmbarkation { get; }
        public TravelEntryMode EntryMode { get; }
        public DateTime EntryDate { get; }
        public DateTime ShnEndDate { get; private set; }
        public SHNFacility ShnFacility { get; private set; }
        public SHNTier Tier { get; private set; }
        public SHNConditions Conditions { get; private set; }
        public TravelEntryStatus Status { get; private set; } = TravelEntryStatus.Incomplete;
        public bool IsPaid { get; internal set; }

        public TravelEntry(
            [NotNull] Person travelPerson,
            [NotNull]  string lastCountryOfEmbarkation, 
            TravelEntryMode entryMode, 
            DateTime entryDate)
        {
            TravelPerson = travelPerson;
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryMode = entryMode;
            EntryDate = entryDate;
            CompleteEntryDetails();
        }

        public TravelEntry(
            [NotNull] Person travelPerson,
            [NotNull] string lastCountryOfEmbarkation, 
            TravelEntryMode entryMode, 
            DateTime entryDate, 
            DateTime shnEndDate,
            [CanBeNull] SHNFacility shnFacility, 
            bool isPaid)
        {
            TravelPerson = travelPerson;
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryMode = entryMode;
            EntryDate = entryDate;
            ShnEndDate = shnEndDate;
            ShnFacility = shnFacility;
            IsPaid = isPaid;
            GetRequirementAndCalculator();
            ValidateCompletedEntry();
        }

        private void CompleteEntryDetails(bool forced = false)
        {
            if (!forced && Status != TravelEntryStatus.Incomplete)
            {
                throw new InvalidOperationException("Record Details has already been calculated or there was an error!");
            }

            GetRequirementAndCalculator();
            ShnEndDate = EntryDate.AddDays(Conditions.QuarantineDays);
            ValidateCompletedEntry();
        }

        private void GetRequirementAndCalculator()
        {
            Tier = SHNTier.FindAppropriateTier(this);
            if (Tier == null)
            {
                Status = TravelEntryStatus.Error;
                throw new InvalidOperationException("Unable to find requirement!");
            }

            Conditions = SHNConditions.FindAppropriateCalculator(this);
            if (Conditions == null)
            {
                Status = TravelEntryStatus.Error;
                throw new InvalidOperationException("Unable to find calculator!");
            }
        }

        private void ValidateCompletedEntry()
        {
            //TODO: Do actual checking
            Status = TravelEntryStatus.Completed;
        }

        public bool AssignSHNFacility(SHNFacility facility)
        {
            if (!Conditions.RequireDedicatedFacility)
            {
                Logging.Error($"No dedicated SHN facility needed for type: {Tier}");
                return false;
            }
            
            ShnFacility = facility;
            return true;
        }

        public double CalculateCharges()
        {
            return Conditions.CalculateCharges(this);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}