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
            throw new NotImplementedException();
        }

        public void ReplaceToken(string no, string location)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}