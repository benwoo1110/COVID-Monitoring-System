using System;

namespace COVIDMonitoringSystem.Core.SafeEntryMgr
{
    public class SafeEntry
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public BusinessLocation Location { get; set; }

        public SafeEntry() { }

        public SafeEntry(DateTime checkIn, BusinessLocation location)
        {
            CheckIn = checkIn;
            Location = location;
        }

        public void PerformCheckOut()
        {
            CheckOut = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Check-in time: {CheckIn, -22} Location: {Location}";
        }
    }
}