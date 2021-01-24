using System;

namespace COVIDMonitoringSystem.Core.SafeEntryMgr
{
    public class SafeEntry
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public BusinessLocation Location { get; set; }

        public SafeEntry(DateTime checkIn, BusinessLocation location)
        {
            CheckIn = checkIn;
            Location = location;
        }

        public void PerformCheckOut()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}