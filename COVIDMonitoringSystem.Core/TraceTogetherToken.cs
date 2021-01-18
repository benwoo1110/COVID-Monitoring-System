using System;

namespace COVIDMonitoringSystem.Core
{
    public class TraceTogetherToken
    {
        public string SerialNo { get; set; }

        public string CollectionLocation { get; set; }

        public DateTime ExpiryDate { get; set; }

        public TraceTogetherToken(string no, string location, DateTime expiry)
        {
            SerialNo = no;
            CollectionLocation = location;
            ExpiryDate = expiry;
        }

        public bool IsEligibleForReplacement()
        {
            throw new NotImplementedException();
        }

        public void ReplaceToken(string no, string location)
        {
            throw new NotImplementedException();
        }
    }
}