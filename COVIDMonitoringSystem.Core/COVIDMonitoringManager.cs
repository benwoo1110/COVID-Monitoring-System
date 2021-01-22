using System;
using System.Collections.Generic;
using System.Globalization;
using COVIDMonitoringSystem.Core.PersonMgr;
using COVIDMonitoringSystem.Core.TravelEntryMgr;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.Core
{
    public class COVIDMonitoringManager
    {
        public List<BusinessLocation> BusinessLocationList { get; private set; }
        public List<SHNFacility> SHNFacilitiesList { get; private set; }
        public List<Person> PersonList { get; private set; }

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
            var locationCsvData = CoreHelper.ReadCsv("resources/BusinessLocation.csv");
            BusinessLocationList = new List<BusinessLocation>();
            foreach (var entry in locationCsvData)
            {
                var newLocation = new BusinessLocation(entry["businessname"], entry["branchcode"], Convert.ToInt32(entry["maximumcapacity"]));
                BusinessLocationList.Add(newLocation);
            }
        }

        private void LoadSHNFacilityData()
        {
            SHNFacilitiesList = (List<SHNFacility>) CoreHelper.FetchFromWeb<List<SHNFacility>>(
                "https://covidmonitoringapiprg2.azurewebsites.net",
                "/facility"
            ) ?? new List<SHNFacility>();
        }

        private void LoadPersonData()
        {
            var personCsvData = CoreHelper.ReadCsv("resources/Person.csv");
            PersonList = new List<Person>();

            foreach (var entry in personCsvData)
            {
                var newPerson = entry["type"] == "resident"
                    ? ParseResidentEntry(entry)
                    : ParseVisitorEntry(entry);

                if (!string.IsNullOrEmpty(entry["travelEntryLastCountry"]))
                {
                    newPerson.AddTravelEntry(ParseTravelEntry(entry, newPerson));
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

        private TravelEntry ParseTravelEntry(IReadOnlyDictionary<string, string> entry, Person newPerson)
        {
            return new TravelEntry(
                newPerson,
                entry["travelEntryLastCountry"],
                CoreHelper.ParseEnum<TravelEntryMode>(entry["travelEntryMode"]),
                Convert.ToDateTime(entry["travelEntryDate"]),
                Convert.ToDateTime(entry["travelShnEndDate"]),
                FindSHNFacility(entry["facilityName"]),
                Convert.ToBoolean(entry["travelIsPaid"])
            );
        }

        public bool GenerateSHNStatusReportFile(DateTime dateTime)
        {
            var csvData = new List<Dictionary<string, string>>();

            PersonList.ForEach(person => person.TravelEntryList.ForEach(entry =>
            {
                if (entry.IsWithinQuarantineTime(dateTime))
                {
                    csvData.Add(GenerateSingleTravelEntryReport(entry));
                }
            }));

            return CoreHelper.WriteCsv(
                $"SHNStatusReport_{dateTime:yyyy-MM-dd_HH-mm-ss}.csv",
                new []{ "Type", "Name", "End Date", "Facility Name" },
                csvData
            );
        }

        private Dictionary<string, string> GenerateSingleTravelEntryReport(TravelEntry entry)
        {
            return new Dictionary<string, string>
            {
                {"Type", ReflectHelper.GetTypeName(entry.TravelPerson)},
                {"Name", entry.TravelPerson.Name},
                {"End Date", entry.ShnEndDate.ToString(CultureInfo.InvariantCulture)},
                {"Facility Name", entry.GetFacilityName()},
            };
        }

        public void AddBusinessLocation(BusinessLocation business)
        {
            BusinessLocationList.Add(business);
        }

        public void AddSHNFacility(SHNFacility facility)
        {
            SHNFacilitiesList.Add(facility);
        }

        public void AddPerson(Person person)
        {
            PersonList.Add(person);
        }

        public BusinessLocation FindBusinessLocation(string name)
        {
            return BusinessLocationList.Find(businessLocation => businessLocation.BusinessName.ToLower().Equals(name.ToLower()));
        }
        
        public SHNFacility FindSHNFacility(string name)
        {
            return SHNFacilitiesList.Find(shnFacility => shnFacility.FacilityName.Equals(name));
        }

        public Person FindPerson(string name)
        {
            return PersonList.Find(person => person.Name.Equals(name));
        }

        public List<T> GetAllPersonOfType<T>() where T : Person
        {
            return PersonList.FindAll(p => p is T).ConvertAll(p => (T) p);
        }
    }
}