//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;

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