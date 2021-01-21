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
        public string EntryMode { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsPaid { get; set; }
        public DateTime ShnEndDate { get; private set; }
        public SHNFacility ShnFacility { get; private set; }
        public ChargeCalculator Calculator { get; private set; }
        public EntryStatus Status { get; private set; } = EntryStatus.Incomplete;

        public TravelEntry()
        {
        }
        
        public TravelEntry(Person travelPerson, string lastCountryOfEmbarkation, string entryMode, DateTime entryDate)
        {
            TravelPerson = travelPerson;
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryMode = entryMode;
            EntryDate = entryDate;
            CalculateRecordDetails();
        }

        public TravelEntry(Person travelPerson, string lastCountryOfEmbarkation, string entryMode, DateTime entryDate, DateTime shnEndDate, SHNFacility shnFacility, bool isPaid)
        {
            TravelPerson = travelPerson;
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryMode = entryMode;
            EntryDate = entryDate;
            ShnEndDate = shnEndDate;
            ShnFacility = shnFacility;
            IsPaid = isPaid;
            Status = EntryStatus.Completed;
            Calculator = ChargeFactory.FindAppropriateCalculator(this);
        }

        public bool CalculateRecordDetails(bool forced = false)
        {
            if (!forced && Status != EntryStatus.Incomplete)
            {
                Logging.Error("Record Details has already been calculated!");
                return false;
            }
            
            Calculator = ChargeFactory.FindAppropriateCalculator(this);
            if (Calculator == null)
            {
                Status = EntryStatus.Error;
                Logging.Error("Unable to find calculator!");
                return false;
            }
            
            ShnEndDate = EntryDate.AddDays(Calculator.QuarantineDays(this));
            return true;
        }

        public double CalculateCharge()
        {
            return ChargeFactory.SwapTestCost + Calculator.TransportCost(this) + Calculator.SDFCost(this);
        }

        public void AssignSHNFacility(SHNFacility facility)
        {
            if (!RequiresSHNFacility())
            {
                throw new InvalidOperationException($"No dedicated SHN facility needed for type: {Calculator.Matcher.Type}");
            }
            
            ShnFacility = facility;
        }

        public bool RequiresSHNFacility()
        {
            return Calculator.Matcher.Type != SHNType.Dedicated;
        }
        
        public override string ToString()
        {
            return base.ToString();
        }
    }
}