//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;

namespace COVIDMonitoringSystem.Core.SafeEntryMgr
{
    public class TraceTogetherToken
    {
        public string SerialNo { get; set; }

        public string CollectionLocation { get; set; }

        public DateTime ExpiryDate { get; set; }

        public TraceTogetherToken(string serialNo, string collectionLocation, DateTime expiryDate)
        {
            SerialNo = serialNo;
            CollectionLocation = collectionLocation;
            ExpiryDate = expiryDate;
        }

        public bool IsEligibleForReplacement()
        {
            var expiryCheck = ((ExpiryDate.Year - DateTime.Now.Year) * 12) + ExpiryDate.Month - DateTime.Now.Month;
            return expiryCheck <= 1;
        }

        public void ReplaceToken(string no, string location)
        {
            SerialNo = no;
            CollectionLocation = location;
            ExpiryDate = DateTime.Now.AddMonths(6);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}