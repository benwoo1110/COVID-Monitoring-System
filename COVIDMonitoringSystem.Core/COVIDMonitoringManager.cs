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
        
        //TODO: Additional feature idea...
        //https://corona.lmao.ninja/v2/countries/singapore

        private void LoadBusinessLocationData()
        {
            //TODO Melvin: LoadBusinessLocationData
            var locationCsvData = Utilities.ReadCsv("resources/BusinessLocation.csv");
        }

        private void LoadSHNFacilityData()
        {
            ShnFacilitiesList = (List<SHNFacility>) Utilities.FetchFromWeb<List<SHNFacility>>(
                "https://covidmonitoringapiprg2.azurewebsites.net",
                "/faciliy"
            ) ?? new List<SHNFacility>();
        }

        private void LoadPersonData()
        {
            var personCsvData = Utilities.ReadCsv("resources/Person.csv");
            PersonList = new List<Person>();

            foreach (var entry in personCsvData)
            {
                Person newPerson = entry["type"] == "resident"
                    ? ParseResidentEntry(entry)
                    : ParseVisitorEntry(entry);

                if (!string.IsNullOrEmpty(entry["travelEntryLastCountry"]))
                {
                    newPerson.AddTravelEntry(ParseTravelEntry(entry));
                }

                PersonList.Add(newPerson);
            }
        }

        private Person ParseResidentEntry(IReadOnlyDictionary<string, string> entry)
        {
            Resident newResident = new Resident(
                entry["name"], 
                entry["address"], 
                Convert.ToDateTime(entry["lastLeftCountry"])
            );

            if (!string.IsNullOrWhiteSpace(entry["tokenSerial"]))
            {
                newResident.Token = ParseTraceTogetherToken(entry);
            }
            
            return newResident;
        }

        private TraceTogetherToken ParseTraceTogetherToken(IReadOnlyDictionary<string, string> entry)
        {
            return new TraceTogetherToken(
                entry["tokenSerial"], 
                entry["tokenCollectionLocation"], 
                Convert.ToDateTime(entry["tokenExpiryDate"])
            );
        }

        private Person ParseVisitorEntry(IReadOnlyDictionary<string, string> entry)
        {
            return new Visitor(entry["name"], entry["passportNo"], entry["nationality"]);
        }

        private TravelEntry ParseTravelEntry(IReadOnlyDictionary<string, string> entry)
        {
            return new TravelEntry(
                entry["travelEntryLastCountry"], 
                entry["travelEntryMode"], 
                Convert.ToDateTime(entry["travelEntryDate"])
            );
        }
    }
}