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
                "/facility"
            ) ?? new List<SHNFacility>();
        }

        private void LoadPersonData()
        {
            var personCsvData = Utilities.ReadCsv("resources/Person.csv");
            PersonList = new List<Person>();

            foreach (var entry in personCsvData)
            {
                var newPerson = entry["type"] == "resident"
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
            var newResident = new Resident(
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
            var newTravelEntry = new TravelEntry(
                entry["travelEntryLastCountry"],
                entry["travelEntryMode"],
                Convert.ToDateTime(entry["travelEntryDate"])
            )
            {
                ShnEndDate = Convert.ToDateTime(entry["travelShnEndDate"]),
                IsPaid = Convert.ToBoolean(entry["travelIsPaid"]),
                ShnFacility = FindSHNFacility(entry["facilityName"])
            };
            
            return newTravelEntry;
        }

        public BusinessLocation FindBusinessLocation(string name)
        {
            return BusinessLocationList.Find(businessLocation => businessLocation.BusinessName.Equals(name));
        }
        
        public SHNFacility FindSHNFacility(string name)
        {
            return ShnFacilitiesList.Find(shnFacility => shnFacility.FacilityName.Equals(name));
        }

        public Person FindPerson(string name)
        {
            return PersonList.Find(person => person.Name.Equals(name));
        }
    }
}