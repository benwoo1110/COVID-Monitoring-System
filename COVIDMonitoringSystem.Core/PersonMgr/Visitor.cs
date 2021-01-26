using System;
using COVIDMonitoringSystem.Core.TravelEntryMgr;

namespace COVIDMonitoringSystem.Core.PersonMgr
{
    public class Visitor : Person
    {
        public string PassportNo { get; set; }
        public string Nationality { get; set; }

        public Visitor(string name, string passportNo, string nationality) : base(name)
        {
            PassportNo = passportNo;
            Nationality = nationality;
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