using System;
using COVIDMonitoringSystem.Core.TravelEntryMgr;

namespace COVIDMonitoringSystem.Core.PersonMgr
{
    public class Resident : Person
    {
        public string Address { get; set; }
        public DateTime LastLeftCountry { get; set; }
        public TraceTogetherToken Token { get; set; }

        public Resident(string name, string address, DateTime lastLeftCountry) : base(name)
        {
            Address = address;
            LastLeftCountry = lastLeftCountry;
        }

        public override SHNChargesReport CalculateSHNCharges()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}