using System;
using System.Collections.Generic;

namespace COVIDMonitoringSystem.Core
{
    public class SafeEntry
    {
        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public BusinessLocation Location { get; set; }

        public SafeEntry(DateTime entry, BusinessLocation location)
        {
            CheckIn = entry;
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