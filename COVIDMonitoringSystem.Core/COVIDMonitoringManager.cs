using System;
using System.Collections.Generic;

namespace COVIDMonitoringSystem.Core
{
    public class COVIDMonitoringManager
    {
        public List<BusinessLocation> BusinessLocationList { get; set; }
        public List<SHNFacility> ShnFacilitiesList { get; set; }
        public List<Person> PersonList { get; set; }

        public COVIDMonitoringManager()
        {
            LoadBusinessLocationData();
            LoadSHNFacilityData();
            LoadPersonData();
        }

        private void LoadBusinessLocationData()
        {
            //TODO Melvin: LoadBusinessLocationData
            var businessCsvData = Utilities.ReadCsv("resources/BusinessLocation.csv");
            BusinessLocationList = new List<BusinessLocation>();

            // Do parsing code here
        }

        private void LoadSHNFacilityData()
        {
            //TODO Ben: LoadSHNFacilityData
            throw new NotImplementedException();
        }

        private void LoadPersonData()
        {
            //TODO Ben: LoadPersonData
            var personCsvData = Utilities.ReadCsv("resources/Person.csv");
            PersonList = new List<Person>();
            
            
        }
    }
}