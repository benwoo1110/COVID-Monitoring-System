//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;

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

        [Obsolete("Required by assignment.")]
        public override double CalculateSHNCharges()
        {
            return GenerateSHNPaymentDetails().TotalPrice;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}