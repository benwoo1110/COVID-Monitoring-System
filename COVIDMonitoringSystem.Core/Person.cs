using System.Collections.Generic;

namespace COVIDMonitoringSystem.Core
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
            TravelEntryList.Add(entry);
        }

        public abstract double CalculateSHNCharges();

        public override string ToString()
        {
            return base.ToString();
        }
    }
}