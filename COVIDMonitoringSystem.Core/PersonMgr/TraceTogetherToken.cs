using System;

namespace COVIDMonitoringSystem.Core.PersonMgr
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
            int expiryCheck = ((ExpiryDate.Year - DateTime.Now.Year) * 12) + ExpiryDate.Month - DateTime.Now.Month;
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