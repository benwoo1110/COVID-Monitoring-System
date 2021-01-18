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
<<<<<<< HEAD
            BusinessLocationList = LoadBusinessLocationData();
            /*ShnFacilitiesList = LoadSHNFacilityData();*/
            PersonList = LoadPersonData();
=======
            LoadBusinessLocationData();
            LoadSHNFacilityData();
            LoadPersonData();
>>>>>>> 5d09f03e31aeadbe3b5b091a87010891ad160523
        }

        private void LoadBusinessLocationData()
        {
            //TODO Melvin: LoadBusinessLocationData
<<<<<<< HEAD
            var locationCsvData = Utilities.ReadCsv("resources/BusinessLocation.csv");
            throw new NotImplementedException();
=======
            var businessCsvData = Utilities.ReadCsv("resources/BusinessLocation.csv");
            BusinessLocationList = new List<BusinessLocation>();

            // Do parsing code here
>>>>>>> 5d09f03e31aeadbe3b5b091a87010891ad160523
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