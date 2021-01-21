using System;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.Utilities;

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

        public TravelEntry(Person travelPerson, string lastCountryOfEmbarkation, TravelEntryMode entryMode, DateTime entryDate)
        {
            TravelPerson = travelPerson;
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryMode = entryMode;
            EntryDate = entryDate;
            CompleteEntryDetails();
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
            Requirement = SHNRequirement.FindAppropriateRequirement(this);
            Calculator = SHNCalculator.FindAppropriateCalculator(this);
            Status = TravelEntryStatus.Completed;
        }

        public bool CompleteEntryDetails(bool forced = false)
        {
            if (!forced && Status != TravelEntryStatus.Incomplete)
            {
                Logging.Error("Record Details has already been calculated or there was an error!");
                return false;
            }
            
            Requirement = SHNRequirement.FindAppropriateRequirement(this);
            if (Requirement == null)
            {
                Status = TravelEntryStatus.Error;
                Logging.Error("Unable to find requirement!");
                return false;
            }
            
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
                cost += SHNCalculator.SwapTestCost;
            }
            if (Requirement.RequiresSHNFacility)
            {
                cost += Calculator.SDFCost(this);
            }
            return cost;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}