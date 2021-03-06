//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

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
            var capacityCheck = MaximumCapacity - VisitorsNow;
            return capacityCheck < 1;
        }

        public override string ToString()
        {
            return BusinessName;
        }
    }
}