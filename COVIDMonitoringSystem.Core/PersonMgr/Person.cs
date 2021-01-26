using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.Core.SafeEntryMgr;
using COVIDMonitoringSystem.Core.TravelEntryMgr;

namespace COVIDMonitoringSystem.Core.PersonMgr
{
    public abstract class Person
    {
        public string  Name { get; set; }
        public List<SafeEntry> SafeEntryList { get; set; }
        public List<TravelEntry> TravelEntryList { get; set; }

        protected Person()
        {
        }

        protected Person(string name)
        {
            Name = name;
            SafeEntryList = new List<SafeEntry>();
            TravelEntryList = new List<TravelEntry>();
        }

        public void AddTravelEntry(TravelEntry entry)
        {
            if (!TravelEntryBelongsToMe(entry))
            {
                throw new ArgumentException($"Travel entry is for {entry.TravelPerson.Name}, not {Name}!");
            }
            
            TravelEntryList.Add(entry);
        }

        public bool TravelEntryBelongsToMe(TravelEntry entry)
        {
            return entry.TravelPerson == this;
        }

        public void AddSafeEntry(SafeEntry entry)
        {
            SafeEntryList.Add(entry);
        }

        public SHNPayment GenerateSHNPaymentDetails()
        {
            return new SHNPayment(this);
        }

        [Obsolete("Required by assignment.")]
        public abstract double CalculateSHNCharges();

        public override string ToString()
        {
            return "Name" + Name;
        }
    }
}