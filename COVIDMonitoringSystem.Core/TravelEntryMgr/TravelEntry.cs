using System;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.TravelEntryMgr;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.Core
{
    public class TravelEntry
    {
        public Person TravelPerson { get; set; }
        public string LastCountryOfEmbarkation { get; set; }
        public TravelEntryMode EntryMode { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsPaid { get; set; }
        public DateTime ShnEndDate { get; private set; }
        public SHNFacility ShnFacility { get; private set; }
        public SHNRequirement Requirement { get; private set; }
        public SHNCalculator Calculator { get; private set; }
        public TravelEntryStatus Status { get; private set; } = TravelEntryStatus.Incomplete;

        public TravelEntry()
        {
        }
        
        public TravelEntry(Person travelPerson, string lastCountryOfEmbarkation, TravelEntryMode entryMode, DateTime entryDate)
        {
            TravelPerson = travelPerson;
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryMode = entryMode;
            EntryDate = entryDate;
            CalculateRecordDetails();
        }

        public TravelEntry(Person travelPerson, string lastCountryOfEmbarkation, TravelEntryMode entryMode, DateTime entryDate, DateTime shnEndDate, SHNFacility shnFacility, bool isPaid)
        {
            TravelPerson = travelPerson;
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryMode = entryMode;
            EntryDate = entryDate;
            ShnEndDate = shnEndDate;
            ShnFacility = shnFacility;
            IsPaid = isPaid;
            Status = TravelEntryStatus.Completed;
            Calculator = SHNCalculator.FindAppropriateCalculator(this);
        }

        public bool CalculateRecordDetails(bool forced = false)
        {
            if (!forced && Status != TravelEntryStatus.Incomplete)
            {
                Logging.Error("Record Details has already been calculated!");
                return false;
            }
            
            Requirement = SHNRequirement.FindAppropriateType(this);
            Calculator = SHNCalculator.FindAppropriateCalculator(this);
            if (Calculator == null)
            {
                Status = TravelEntryStatus.Error;
                Logging.Error("Unable to find calculator!");
                return false;
            }
            
            ShnEndDate = EntryDate.AddDays(Requirement.QuarantineDays);
            return true;
        }

        public double CalculateCharge()
        {
            return SHNCalculator.SwapTestCost + Calculator.TransportCost(this) + Calculator.SDFCost(this);
        }

        public void AssignSHNFacility(SHNFacility facility)
        {
            if (!Requirement.RequiresSHNFacility)
            {
                throw new InvalidOperationException($"No dedicated SHN facility needed for type: {Requirement}");
            }
            
            ShnFacility = facility;
        }

        public bool ValidateFacilityStatus()
        {
            return Requirement.RequiresSHNFacility
                ? ShnFacility != null
                : ShnFacility == null;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}