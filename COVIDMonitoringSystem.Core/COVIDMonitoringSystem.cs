using System;
using System.Collections.Generic;

namespace COVIDMonitoringSystem.Core
{
    public class COVIDMonitoringSystem
    {
        public List<SHNFacility> ShnFacilitiesList { get; }
        public List<Person> PersonList { get; }
        public List<BusinessLocation> BusinessLocationList { get; }
        
        public COVIDMonitoringSystem()
        {
            ShnFacilitiesList = LoadSHNFacilityData();
            PersonList = LoadPersonData();
            BusinessLocationList = LoadBusinessLocationData();
        }
        
        private List<SHNFacility> LoadSHNFacilityData()
        {
            //TODO: LoadSHNFacilityData
            throw new NotImplementedException();
        }
        
        private List<Person> LoadPersonData()
        {
            //TODO: LoadPersonData
            throw new NotImplementedException();
        }

        private List<BusinessLocation> LoadBusinessLocationData()
        {
            //TODO: LoadBusinessLocationData
            throw new NotImplementedException();
        }
    }
}