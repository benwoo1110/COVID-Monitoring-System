//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using COVIDMonitoringSystem.Core.PersonMgr;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class TravelEntry
    {
        public Person TravelPerson { get; }
        public string LastCountryOfEmbarkation { get; }
        public TravelEntryMode EntryModeType { get; }
        public DateTime EntryDate { get; }
        public DateTime ShnEndDate { get; private set; }
        public SHNFacility ShnStay { get; private set; }
        public SHNTier Tier { get; private set; }
        public SHNConditions Conditions { get; private set; }
        public TravelEntryStatus Status { get; private set; } = TravelEntryStatus.Incomplete;
        public bool IsPaid { get; internal set; }

        [Obsolete("Required by assignment.")]
        public string EntryMode => Enum.GetName(typeof(TravelEntryMode), EntryModeType);
        
        public TravelEntry()
        {
        }

        public TravelEntry(
            [NotNull] Person travelPerson,
            [NotNull]  string lastCountryOfEmbarkation, 
            TravelEntryMode entryModeType, 
            DateTime entryDate)
        {
            TravelPerson = travelPerson;
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryModeType = entryModeType;
            EntryDate = entryDate;
            CompleteEntryDetails();
        }

        public TravelEntry(
            [NotNull] Person travelPerson,
            [NotNull] string lastCountryOfEmbarkation, 
            TravelEntryMode entryModeType, 
            DateTime entryDate, 
            DateTime shnEndDate,
            [CanBeNull] SHNFacility shnStay, 
            bool isPaid)
        {
            TravelPerson = travelPerson;
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryModeType = entryModeType;
            EntryDate = entryDate;
            ShnEndDate = shnEndDate;
            ShnStay = shnStay;
            IsPaid = isPaid;
            GetRequirementAndCalculator();
            ValidateCompletedEntry();
            UpdateFacilityVacancy();
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

        private void UpdateFacilityVacancy()
        {
            if (!Conditions.RequireDedicatedFacility)
            {
                return;
            }

            ShnStay.FacilityVacancy--;
        }
        
        public bool AssignSHNFacility(SHNFacility facility)
        {
            if (!Conditions.RequireDedicatedFacility)
            {
                throw new InvalidOperationException($"No dedicated SHN facility needed for tier: {Tier}");
            }

            if (ShnStay != null)
            {
                return false;
            }
            
            ShnStay = facility;
            UpdateFacilityVacancy();
            return true;
        }

        public double CalculateCharges()
        {
            return Conditions.CalculateCharges(this);
        }

        public bool IsWithinQuarantineTime(DateTime dateTime)
        {
            if (EntryDate.Equals(ShnEndDate))
            {
                return false;
            }
            return dateTime >= EntryDate && dateTime <= ShnEndDate;
        }

        public string GetFacilityName()
        {
            if (Tier == SHNTier.None)
            {
                return "";
            }
            if (Tier == SHNTier.OwnAcc)
            {
                return "Own Accommodation";
            }
            return ShnStay.FacilityName;
        }

        public override string ToString()
        {
            return $"{LastCountryOfEmbarkation,-18} {EntryDate}";
        }
    }
}