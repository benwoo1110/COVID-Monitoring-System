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
        public SHNRequirement Requirement { get; private set; }
        public SHNCalculator Calculator { get; private set; }
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
            ShnEndDate = EntryDate.AddDays(Requirement.QuarantineDays);
            ValidateCompletedEntry();
        }

        private void GetRequirementAndCalculator()
        {
            Requirement = SHNRequirement.FindAppropriateRequirement(this);
            if (Requirement == null)
            {
                Status = TravelEntryStatus.Error;
                throw new InvalidOperationException("Unable to find requirement!");
            }

            Calculator = SHNCalculator.FindAppropriateCalculator(this);
            if (Calculator == null)
            {
                Status = TravelEntryStatus.Error;
                throw new InvalidOperationException("Unable to find calculator!");
            }
        }

        private void ValidateCompletedEntry()
        {
            Status = TravelEntryStatus.Completed;
        }

        public bool AssignSHNFacility(SHNFacility facility)
        {
            if (!Requirement.RequiresSHNFacility)
            {
                Logging.Error($"No dedicated SHN facility needed for type: {Requirement}");
                return false;
            }
            
            ShnFacility = facility;
            return true;
        }

        public double CalculateCharge()
        {
            var cost = Calculator.TransportCost(this);
            if (Requirement.NeedSwapTest)
            {
                cost += Calculator.SwapTestCost(this);
            }
            if (Requirement.RequiresSHNFacility)
            {
                cost += Calculator.DedicatedFacilityCost(this);
            }
            return cost;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}