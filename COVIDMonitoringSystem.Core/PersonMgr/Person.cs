using System;
using System.Collections.Generic;
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
            if (entry.TravelPerson != this)
            {
                throw new ArgumentException($"Travel entry is for {entry.TravelPerson.Name}, not {Name}!");
            }
            
            TravelEntryList.Add(entry);
        }

        public void AddSafeEntry(SafeEntry entry)
        {
            SafeEntryList.Add(entry);
        }

        public abstract SHNPayment CalculateSHNCharges();

        public override string ToString()
        {
            return base.ToString();
        }
    }
}