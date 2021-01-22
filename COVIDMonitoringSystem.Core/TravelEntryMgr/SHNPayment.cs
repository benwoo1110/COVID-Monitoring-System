using System;
using System.Collections.Generic;
using System.Linq;
using COVIDMonitoringSystem.Core.PersonMgr;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class SHNPayment
    {
        public Person PaymentPerson { get; }
        public IReadOnlyList<TravelEntry> Entries { get; }

        public double SubTotalPrice { get; private set; }
        
        public double TotalPrice { get; private set; }

        internal SHNPayment(Person paymentPerson)
        {
            PaymentPerson = paymentPerson;
            Entries = PaymentPerson.TravelEntryList.FindAll(tr => !tr.IsPaid);
            SetUpPaymentDetails();
        }

        private void SetUpPaymentDetails()
        {
            SubTotalPrice = Entries.Sum(travelEntry => travelEntry.CalculateCharges());
            TotalPrice = SubTotalPrice * 1.07;
        }

        public int NumberOfUnpaidEntries()
        {
            return Entries.Count;
        }

        public void DoPayment()
        {
            ValidateIntegrity();
            foreach (var travelEntry in Entries)
            {
                travelEntry.IsPaid = true;
            }
        }

        public void ValidateIntegrity()
        {
            foreach (var entry in Entries)
            {
                if (!PaymentPerson.TravelEntryBelongsToMe(entry))
                {
                    throw new InvalidOperationException($"Travel entry is for {entry.TravelPerson.Name}, not {PaymentPerson.Name}!");
                }
                if (entry.IsPaid)
                {
                    throw new InvalidOperationException("Travel entry has already been paid!");
                }
            }
        }
    }
}