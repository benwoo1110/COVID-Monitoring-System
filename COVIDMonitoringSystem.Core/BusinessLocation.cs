using System;

namespace COVIDMonitoringSystem.Core
{
    public class BusinessLocation
    {
        public string BusinessName { get; set; }

        public string BranchCode { get; set; }

        public int MaximumCapacity { get; set; }

        public int VisitorsNow { get; set; }

        public BusinessLocation(string name, string branch, int capacity)
        {
            BusinessName = name;
            BranchCode = branch;
            VisitorsNow = capacity;
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