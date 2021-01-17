using System;
using System.Collections.Generic;

namespace COVIDMonitoringSystem.Core
{
    public class COVIDMonitoringManager
    {
        public List<BusinessLocation> BusinessLocationList { get; }
        public List<SHNFacility> ShnFacilitiesList { get; }
        public List<Person> PersonList { get; }

        public COVIDMonitoringManager()
        {
            /*BusinessLocationList = LoadBusinessLocationData();
            ShnFacilitiesList = LoadSHNFacilityData();*/
            PersonList = LoadPersonData();
        }

        private List<BusinessLocation> LoadBusinessLocationData()
        {
            //TODO Melvin: LoadBusinessLocationData
            throw new NotImplementedException();
        }
        
        private List<SHNFacility> LoadSHNFacilityData()
        {
            //TODO Melvin: LoadSHNFacilityData
            throw new NotImplementedException();
        }
        
        private List<Person> LoadPersonData()
        {
            //TODO Ben: LoadPersonData
            var personCsvData = Utilities.ReadCsv("resources/Person.csv");

            throw new NotImplementedException();
        }
    }
}