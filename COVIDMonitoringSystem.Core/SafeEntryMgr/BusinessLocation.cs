using System;

namespace COVIDMonitoringSystem.Core.SafeEntryMgr
{
    public class BusinessLocation
    {
        public string BusinessName { get; set; }
        public string BranchCode { get; set; }
        public int MaximumCapacity { get; set; }

        public int VisitorsNow { get; set; }

        public BusinessLocation(string businessName, string branchCode, int maximumCapacity)
        {
            BusinessName = businessName;
            BranchCode = branchCode;
            MaximumCapacity = maximumCapacity;
        }

        public bool IsFull()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}